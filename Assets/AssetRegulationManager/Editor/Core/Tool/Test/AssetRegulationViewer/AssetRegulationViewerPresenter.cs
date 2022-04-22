// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.Test.AssetRegulationViewer
{
    internal sealed class AssetRegulationViewerPresenter
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly AssetRegulationViewerState _state;
        private readonly AssetRegulationTestStore _store;
        private CompositeDisposable _currentTestCollectionDisposables = new CompositeDisposable();
        private bool _needReloading;
        private bool _needRepainting;
        private AssetRegulationViewerTreeView _treeView;
        private AssetRegulationViewerWindow _window;

        public AssetRegulationViewerPresenter(AssetRegulationTestStore store, AssetRegulationViewerState state)
        {
            _store = store;
            _state = state;
        }

        public void Dispose()
        {
        }

        public void Setup(AssetRegulationViewerWindow window)
        {
            _window = window;
            _treeView = _window.TreeView;

            _state.SelectedAssetPath.Subscribe(x =>
            {
                _window.SelectedAssetPath = x;
                var selectionObj = AssetDatabase.LoadAssetAtPath<Object>(x);
                EditorGUIUtility.PingObject(selectionObj);
            }).DisposeWith(_disposables);

            _state.TestFilterType.Subscribe(x =>
            {
                var excludeEmptyTests = x == AssetRegulationTestStoreFilter.ExcludeEmptyTests;
                _window.ExcludeEmptyTests.SetValueAndNotNotify(excludeEmptyTests);
            }).DisposeWith(_disposables);

            _store.FilteredTests.ObservableAdd
                .Subscribe(x =>
                {
                    AddTreeViewItem(x.Value);
                    RequestTreeViewReloading();
                })
                .DisposeWith(_disposables);

            _store.FilteredTests.ObservableClear
                .Subscribe(_ =>
                {
                    ClearItems();
                    RequestTreeViewReloading();
                })
                .DisposeWith(_disposables);
        }

        public void Cleanup()
        {
            _disposables.Dispose();
        }

        private void ClearItems()
        {
            if (_currentTestCollectionDisposables != null) _currentTestCollectionDisposables.Dispose();

            _currentTestCollectionDisposables = new CompositeDisposable();

            _treeView.ClearItems();
        }

        private void AddTreeViewItem(AssetRegulationTest assetRegulationTest)
        {
            var assetPath = assetRegulationTest.AssetPath;
            var assetPathTreeViewItem = _treeView.AddAssetRegulationTestTreeViewItem(assetPath,
                assetRegulationTest.Id, assetRegulationTest.LatestStatus.Value);
            assetRegulationTest.LatestStatus.Subscribe(x =>
                {
                    assetPathTreeViewItem.Status = x;
                    RequestTreeViewRepainting();
                })
                .DisposeWith(_currentTestCollectionDisposables);

            foreach (var entry in assetRegulationTest.Entries.Values)
            {
                var assetRegulationTreeViewItem =
                    _treeView.AddAssetRegulationTestEntryTreeViewItem(entry.Id, entry.Description, entry.Status.Value,
                        assetPathTreeViewItem.id);
                entry.Status.Subscribe(x =>
                    {
                        assetRegulationTreeViewItem.Status = x;
                        _treeView.Repaint();
                        RequestTreeViewRepainting();
                    })
                    .DisposeWith(_currentTestCollectionDisposables);
                entry.Message.Subscribe(x =>
                    {
                        assetRegulationTreeViewItem.ActualValue = x;
                        RequestTreeViewRepainting();
                    })
                    .DisposeWith(_currentTestCollectionDisposables);
            }
        }

        private void RequestTreeViewReloading()
        {
            if (_needReloading)
                return;

            _needReloading = true;
            EditorApplication.delayCall += () =>
            {
                if (!_needReloading)
                    return;

                _treeView.Reload();
                _needReloading = false;
            };
        }

        private void RequestTreeViewRepainting()
        {
            if (_needRepainting)
                return;

            _needRepainting = true;
            EditorApplication.delayCall += () =>
            {
                if (!_needRepainting)
                    return;

                _treeView.Repaint();
                _needRepainting = false;
            };
        }
    }
}

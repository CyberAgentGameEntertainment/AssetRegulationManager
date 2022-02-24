// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.IO;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationViewer
{
    internal sealed class AssetRegulationViewerPresenter
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly AssetRegulationManagerStore _store;
        private CompositeDisposable _currentTestCollectionDisposables = new CompositeDisposable();
        private AssetRegulationViewerTreeView _treeView;
        private AssetRegulationViewerWindow _window;

        public AssetRegulationViewerPresenter(AssetRegulationManagerStore store)
        {
            _store = store;
        }

        public void Dispose()
        {
        }

        public void Setup(AssetRegulationViewerWindow window, AssetRegulationViewerState state)
        {
            _window = window;
            _treeView = _window.TreeView;
            
            state.SelectedAssetPath.Subscribe(x =>
            {
                _window.SelectedAssetPath = x;
                var selectionObj = AssetDatabase.LoadAssetAtPath<Object>(x);
                EditorGUIUtility.PingObject(selectionObj);
            }).DisposeWith(_disposables);
            state.TestSortType.Subscribe(x =>
            {
                var excludeEmptyTests = x == TestSortType.ExcludeEmptyTests;
                _window.ExcludeEmptyTests.SetValueAndNotNotify(excludeEmptyTests);
            }).DisposeWith(_disposables);

            _store.SortedTests.ObservableAdd.Subscribe(x => AddTreeViewItem(x.Value)).DisposeWith(_disposables);
            _store.SortedTests.ObservableClear.Subscribe(_ => ClearItems()).DisposeWith(_disposables);
        }

        public void Cleanup()
        {
            _disposables.Dispose();
        }

        private void ClearItems()
        {
            if (_currentTestCollectionDisposables != null)
            {
                _currentTestCollectionDisposables.Dispose();
            }

            _currentTestCollectionDisposables = new CompositeDisposable();

            _treeView.ClearItems();
        }

        private void AddTreeViewItem(AssetRegulationTest assetRegulationTest)
        {
            var assetPath = assetRegulationTest.AssetPath;
            var assetName = Path.GetFileNameWithoutExtension(assetPath);
            var icon = (Texture2D)AssetDatabase.GetCachedIcon(assetPath);
            var assetPathTreeViewItem = _treeView.AddAssetRegulationTestTreeViewItem(assetName,
                assetRegulationTest.Id, assetRegulationTest.LatestStatus.Value, icon);
            assetRegulationTest.LatestStatus.Subscribe(x => assetPathTreeViewItem.Status = x)
                .DisposeWith(_currentTestCollectionDisposables);

            foreach (var entry in assetRegulationTest.Entries.Values)
            {
                var assetRegulationTreeViewItem =
                    _treeView.AddAssetRegulationTestEntryTreeViewItem(entry.Id, entry.Description, entry.Status.Value,
                        assetPathTreeViewItem.id);
                entry.Status.Subscribe(x => assetRegulationTreeViewItem.Status = x)
                    .DisposeWith(_currentTestCollectionDisposables);
                entry.Message.Subscribe(x => assetRegulationTreeViewItem.ActualValue = x)
                    .DisposeWith(_currentTestCollectionDisposables);
            }
        }
    }
}

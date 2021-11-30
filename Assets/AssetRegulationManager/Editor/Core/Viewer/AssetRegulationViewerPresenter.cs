// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.Observable;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class RegulationViewerPresenter
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly RegulationManagerStore _store;
        private CompositeDisposable _currentTestCollectionDisposables = new CompositeDisposable();
        private RegulationTreeView _treeView;
        private RegulationViewerWindow _window;

        internal RegulationViewerPresenter(RegulationManagerStore store)
        {
            _store = store;
        }

        internal void Dispose()
        {
            _disposables.Dispose();
        }

        internal void Setup(RegulationViewerWindow window)
        {
            _window = window;
            _treeView = _window.TreeView;

            _store.Tests.ObservableAdd.Subscribe(x => AddTreeViewItem(x.Value)).DisposeWith(_disposables);
            _store.Tests.ObservableClear.Subscribe(_ => ClearItems()).DisposeWith(_disposables);
        }

        private void ClearItems()
        {
            if (_currentTestCollectionDisposables != null) _currentTestCollectionDisposables.Dispose();
            _currentTestCollectionDisposables = new CompositeDisposable();

            _treeView.ClearItems();
        }

        private void AddTreeViewItem(AssetRegulationTest assetRegulationTest)
        {
            var assetPathTreeViewItem = _treeView.AddAssetPathTreeViewItem(assetRegulationTest.AssetPath);
            foreach (var entry in assetRegulationTest.Entries)
            {
                var assetRegulationTreeViewItem =
                    _treeView.AddAssetRegulationTreeViewItem(entry.Description, entry.Status.Value,
                        assetPathTreeViewItem.id);
                entry.Status.Subscribe(x => assetRegulationTreeViewItem.Status = x)
                    .DisposeWith(_currentTestCollectionDisposables);
            }
        }
    }
}
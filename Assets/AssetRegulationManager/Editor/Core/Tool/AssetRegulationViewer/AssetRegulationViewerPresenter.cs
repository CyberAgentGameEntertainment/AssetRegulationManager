// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.Observable;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationViewer
{
    internal sealed class AssetRegulationViewerPresenter
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly AssetRegulationManagerStore _store;
        private CompositeDisposable _currentTestCollectionDisposables = new CompositeDisposable();
        private AssetRegulationTreeView _treeView;
        private AssetRegulationViewerWindow _window;

        internal AssetRegulationViewerPresenter(AssetRegulationManagerStore store)
        {
            _store = store;
        }

        internal void Dispose()
        {
            _disposables.Dispose();
        }

        internal void Setup(AssetRegulationViewerWindow window)
        {
            _window = window;
            _treeView = _window.TreeView;

            _store.Tests.ObservableAdd.Subscribe(x => AddTreeViewItem(x.Value)).DisposeWith(_disposables);
            _store.Tests.ObservableClear.Subscribe(_ => ClearItems()).DisposeWith(_disposables);
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
            var assetPathTreeViewItem = _treeView.AddAssetRegulationTestTreeViewItem(assetRegulationTest.AssetPath,
                assetRegulationTest.Id, assetRegulationTest.LatestStatus.Value);
            assetRegulationTest.LatestStatus.Subscribe(x => assetPathTreeViewItem.Status = x)
                .DisposeWith(_currentTestCollectionDisposables);

            foreach (var entry in assetRegulationTest.Entries.Values)
            {
                var assetRegulationTreeViewItem =
                    _treeView.AddAssetRegulationTestEntryTreeViewItem(entry.Id, entry.Description, entry.Status.Value,
                        assetPathTreeViewItem.id);
                entry.Status.Subscribe(x => assetRegulationTreeViewItem.Status = x)
                    .DisposeWith(_currentTestCollectionDisposables);
            }
        }
    }
}

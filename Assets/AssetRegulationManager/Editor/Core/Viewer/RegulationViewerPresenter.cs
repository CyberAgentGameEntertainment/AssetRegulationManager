// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Foundation.Observable;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class RegulationViewerPresenter
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly RegulationViewerStore _store;
        private RegulationTreeView _treeView;
        private RegulationViewerWindow _window;
        private CompositeDisposable _currentTestCollectionDisposables;

        internal RegulationViewerPresenter(RegulationViewerStore store)
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
            
            _store.TestCollection.Subscribe(AddTreeViewItem).DisposeWith(_disposables);
        }

        private void AddTreeViewItem(TestCollection testCollection)
        {
            if (testCollection == null)
                return;
            
            if (_currentTestCollectionDisposables != null)
            {
                _currentTestCollectionDisposables.Dispose();
            }
            _currentTestCollectionDisposables = new CompositeDisposable();
            
            _treeView.ClearItems();

            foreach (var pathGroup in testCollection.EntryData.GroupBy(x => x.Path))
            {
                var assetPathTreeViewItem = _treeView.AddAssetPathTreeViewItem(pathGroup.Key);
                foreach (var entryDatum in pathGroup)
                {
                    var assetRegulationTreeViewItem = _treeView.AddAssetRegulationTreeViewItem(entryDatum, assetPathTreeViewItem.id);
                    entryDatum.ResultType.Subscribe(x => assetRegulationTreeViewItem.ResultType = x).DisposeWith(_currentTestCollectionDisposables);
                }
            }
        }
    }
}
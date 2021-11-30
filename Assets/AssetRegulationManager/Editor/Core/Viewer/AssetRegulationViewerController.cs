// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.Observable;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class AssetRegulationViewerController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly AssetRegulationTestGenerateService _generateService;
        private readonly AssetRegulationManagerStore _store;
        private AssetRegulationTreeView _treeView;
        private AssetRegulationViewerWindow _window;

        internal AssetRegulationViewerController(AssetRegulationManagerStore store)
        {
            _store = store;
            _generateService = new AssetRegulationTestGenerateService(store);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        internal void Setup(AssetRegulationViewerWindow window)
        {
            _window = window;
            _treeView = _window.TreeView;

            window.AssetPathOrFilterObservable.Subscribe(_generateService.Run).DisposeWith(_disposables);
            window.CheckAllButtonClickedObservable
                .Subscribe(_ => CheckAll())
                .DisposeWith(_disposables);
            window.CheckSelectedAddButtonClickedObservable.Subscribe(_ => CheckSelected())
                .DisposeWith(_disposables);
        }

        private void CheckAll()
        {
            foreach (var test in _store.Tests) test.RunAll();
        }

        private void CheckSelected()
        {
            var selectionEntryIds = _treeView.SelectionAssetRegulationTestIndex().ToList();
            
            foreach (var test in _store.Tests) test.RunSelection(selectionEntryIds);
        }
    }
}
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
    internal sealed class RegulationViewerController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly AssetRegulationTestGenerateService _testGenerateService;
        private readonly RegulationManagerStore _store;
        private RegulationTreeView _treeView;
        private RegulationViewerWindow _window;

        internal RegulationViewerController(RegulationManagerStore store)
        {
            _store = store;
            _testGenerateService = new AssetRegulationTestGenerateService(store);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        internal void Setup(RegulationViewerWindow window)
        {
            _window = window;
            _treeView = _window.TreeView;

            window.AssetPathOrFilterObservable.Subscribe(_testGenerateService.Run).DisposeWith(_disposables);
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
            foreach (var assetRegulationTestIndexGroup in
                _treeView.SelectionAssetRegulationTestIndex().GroupBy(x => x.TestIndex))
                _store.Tests[assetRegulationTestIndexGroup.Key]
                    .RunSelection(assetRegulationTestIndexGroup.Select(x => x.TestEntryIndex));
        }
    }
}
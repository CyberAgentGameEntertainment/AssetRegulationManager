// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Linq;
using AssetRegulationManager.Editor.Foundation.Observable;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class RegulationViewerController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly AssetRegulationTestGenerateService _testGenerateService;
        private readonly RunTestService _runTestService;
        private RegulationViewerWindow _window;
        private RegulationTreeView _treeView;

        internal RegulationViewerController(AssetRegulationTestGenerateService testGenerateService, RunTestService runTestService)
        {
            _testGenerateService = testGenerateService;
            _runTestService = runTestService;
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
            window.CheckAllButtonClickedObservable.Subscribe(_ => _runTestService.Run(_treeView.AllAssetRegulationTreeViewItem().Select(x => x.MetaDatum))).DisposeWith(_disposables);
            window.CheckSelectedAddButtonClickedObservable.Subscribe(_ => _runTestService.Run(_treeView.SelectionAssetRegulationTreeViewItem().Select(x => x.MetaDatum))).DisposeWith(_disposables);
        }
    }
}
// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Foundation.Observable;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class RegulationViewerPresenter
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly RegulationViewerModel _model;
        private RegulationTreeView _treeView;
        private RegulationViewerWindow _window;

        internal RegulationViewerPresenter(RegulationViewerModel model)
        {
            _model = model;
        }

        internal void Dispose()
        {
            _disposables.Dispose();
        }

        internal void Setup(RegulationViewerWindow window)
        {
            _window = window;
            _treeView = window.TreeView;

            _model.FormatViewDataObservable.Subscribe(_treeView.AddTreeViewItem).DisposeWith(_disposables);
        }
    }
}
// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Foundation.Observable;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal class RegulationViewerPresenter
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly RegulationViewerModel _model;
        private RegulationViewerWindow _window;
        private RegulationTreeView _treeView;

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
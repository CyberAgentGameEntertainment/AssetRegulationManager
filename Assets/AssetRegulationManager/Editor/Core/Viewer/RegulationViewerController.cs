// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Foundation.Observable;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class RegulationViewerController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly RegulationViewerModel _model;
        private RegulationViewerWindow _window;

        internal RegulationViewerController(RegulationViewerModel model)
        {
            _model = model;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        internal void Setup(RegulationViewerWindow window)
        {
            _window = window;

            window.SearchAssetButtonClickedObservable.Subscribe(_model.SearchAssets).DisposeWith(_disposables);
            // window.CheckAllButtonClickedObservable.Subscribe(x => );
            // window.CheckSelectedAddButtonClickedObservable.Subscribe(x => );
        }
    }
}
// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Foundation.Observable;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    public class RegulationViewerController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly RegulationViewerModel _model;
        private RegulationViewerWindow _window;

        public RegulationViewerController(RegulationViewerModel model)
        {
            _model = model;
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
        
        public void Setup(RegulationViewerWindow window)
        {
            _window = window;

            window.SearchAssetButtonClickedObservable.Subscribe(_model.SearchAssets).DisposeWith(_disposables);;
            // window.CheckAllButtonClickedObservable.Subscribe(x => );
            // window.CheckSelectedAddButtonClickedObservable.Subscribe(x => );
        }
    }
}
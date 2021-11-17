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
            
            _model.FormatViewDataObservable.Subscribe(FormatViewData).DisposeWith(_disposables);
            // _model.TestResultObservable.Subscribe()
        }

        // TODO: 消す
        private void FormatViewData(IEnumerable<RegulationViewDatum> enumerable)
        {
            foreach (var x in enumerable)
            {
                Debug.Log(x.Path);
                foreach (var y in x.EntryViewData)
                {
                    Debug.Log(y.Id);
                    Debug.Log(y.Index);
                    Debug.Log(y.Explanation);
                    Debug.Log(y.ResultType);
                }
            }
        }
    }
}
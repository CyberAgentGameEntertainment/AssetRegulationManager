// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Tool.AssetRegulationViewer;

namespace AssetRegulationManager.Editor.Core
{
    internal sealed class AssetRegulationViewerApplication : IDisposable
    {
        private static int _referenceCount;
        private static AssetRegulationViewerApplication _instance;

        private AssetRegulationViewerApplication()
        {
            var repository = new AssetRegulationRepository();
            var regulations = repository.GetAllRegulations().ToList();
            var store = new AssetRegulationManagerStore(regulations);
            AssetRegulationViewerPresenter = new AssetRegulationViewerPresenter(store);
            AssetRegulationViewerController = new AssetRegulationViewerController(store);
        }

        internal AssetRegulationViewerPresenter AssetRegulationViewerPresenter { get; }
        internal AssetRegulationViewerController AssetRegulationViewerController { get; }

        public void Dispose()
        {
            AssetRegulationViewerPresenter.Dispose();
            AssetRegulationViewerController.Dispose();
        }

        internal static AssetRegulationViewerApplication RequestInstance()
        {
            if (_referenceCount++ == 0) _instance = new AssetRegulationViewerApplication();
            _referenceCount++;

            return _instance;
        }

        internal static void ReleaseInstance()
        {
            _referenceCount--;
            if (_referenceCount == 0)
            {
                _instance.Dispose();
                _instance = null;
            }
        }
    }
}
// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Tool.AssetRegulationViewer;

namespace AssetRegulationManager.Editor.Core
{
    internal sealed class AssetRegulationManagerApplication : IDisposable
    {
        private static int _referenceCount;
        private static AssetRegulationManagerApplication _instance;

        private AssetRegulationManagerApplication()
        {
            var repository = new AssetRegulationRepository();
            var store = new AssetRegulationManagerStore(repository);
            AssetRegulationViewerPresenter = new AssetRegulationViewerPresenter(store);
            AssetRegulationViewerController = new AssetRegulationViewerController(store);
        }

        public AssetRegulationViewerPresenter AssetRegulationViewerPresenter { get; }
        public AssetRegulationViewerController AssetRegulationViewerController { get; }

        public void Dispose()
        {
            AssetRegulationViewerPresenter.Dispose();
            AssetRegulationViewerController.Dispose();
        }

        public static AssetRegulationManagerApplication RequestInstance()
        {
            if (_referenceCount++ == 0)
            {
                _instance = new AssetRegulationManagerApplication();
            }

            return _instance;
        }

        public static void ReleaseInstance()
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

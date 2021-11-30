// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Core.Model;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class AssetRegulationViewerApplication : IDisposable
    {
        private static int _referenceCount;
        private static AssetRegulationViewerApplication _instance;

        private AssetRegulationViewerApplication()
        {
            var regulationCollection =
                AssetDatabase.LoadAssetAtPath<AssetRegulationCollection>(
                    "Assets/AssetRegulationManagerMasterData/Editor/AssetRegulationCollection.asset");

            var store = new AssetRegulationManagerStore(regulationCollection.Regulations);
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
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
            // TODO: 保存場所を決めかねているため、決め次第実装
            var regulationCollection =
                AssetDatabase.LoadAssetAtPath<AssetRegulationCollection>(
                    "Assets/Develop/AssetRegulationCollection.asset");

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

            return _instance;
        }

        internal static void ReleaseInstance()
        {
            if (--_referenceCount == 0)
            {
                _instance.Dispose();
                _instance = null;
            }
        }
    }
}
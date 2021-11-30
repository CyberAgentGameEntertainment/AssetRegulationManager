// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Core.Model;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class RegulationViewerApplication : IDisposable
    {
        private static int _referenceCount;
        private static RegulationViewerApplication _instance;

        private RegulationViewerApplication()
        {
            // TODO: 保存場所を決めかねているため、決め次第実装、おそらくStreamingAssetsPath
            var regulationCollection =
                AssetDatabase.LoadAssetAtPath<AssetRegulationCollection>(
                    "Assets/Develop/AssetRegulationCollection.asset");

            var store = new RegulationManagerStore(regulationCollection.Regulations);
            RegulationViewerPresenter = new RegulationViewerPresenter(store);
            RegulationViewerController = new RegulationViewerController(store);
        }

        internal RegulationViewerPresenter RegulationViewerPresenter { get; }
        internal RegulationViewerController RegulationViewerController { get; }

        public void Dispose()
        {
            RegulationViewerPresenter.Dispose();
            RegulationViewerController.Dispose();
        }

        internal static RegulationViewerApplication RequestInstance()
        {
            if (_referenceCount++ == 0) _instance = new RegulationViewerApplication();

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
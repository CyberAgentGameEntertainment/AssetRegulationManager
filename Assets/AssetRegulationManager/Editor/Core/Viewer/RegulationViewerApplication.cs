// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    public sealed class RegulationViewerApplication : IDisposable
    {
        private static int _referenceCount;
        private static RegulationViewerApplication _instance;

        private RegulationViewerApplication()
        {
            // TODO: 読み込みは仮
            var regulationCollection =
                AssetDatabase.LoadAssetAtPath<AssetRegulationCollection>(
                    "Assets/Develop/AssetRegulationCollection.asset");
            var model = new RegulationViewerModel(regulationCollection.Regulations);

            RegulationViewerPresenter = new RegulationViewerPresenter(model);
            RegulationViewerController = new RegulationViewerController(model);
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
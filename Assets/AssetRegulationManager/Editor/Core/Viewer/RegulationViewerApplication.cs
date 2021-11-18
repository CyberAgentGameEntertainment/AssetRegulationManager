// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    public class RegulationViewerApplication : IDisposable
    {
        private static int _referenceCount;
        private static RegulationViewerApplication _instance;

        private RegulationViewerApplication()
        {
            // TODO: 読み込みは仮
            var regulationCollection =  AssetDatabase.LoadAssetAtPath<AssetRegulationCollection>("Assets/IgnoreTmp/Asset Regulation Collection.asset");
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
            if (_referenceCount++ == 0)
            {
                _instance = new RegulationViewerApplication();
            }

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
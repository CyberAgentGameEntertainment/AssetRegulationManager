// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Tool.AssetRegulationViewer;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core
{
    internal sealed class AssetRegulationManagerApplication : IDisposable
    {
        private const string TestFilterTypeKey = "TestFilterType";
        private static int _referenceCount;
        private static AssetRegulationManagerApplication _instance;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();


        private AssetRegulationManagerApplication()
        {
            var repository = new AssetRegulationRepository();
            var store = new AssetRegulationManagerStore(repository);

            AssetRegulationViewerState = new AssetRegulationViewerState();
            var filterType =
                (AssetRegulationTestStoreFilter)EditorPrefs.GetInt(TestFilterTypeKey,
                    (int)AssetRegulationTestStoreFilter.ExcludeEmptyTests);
            AssetRegulationViewerState.TestFilterType.Value = filterType;
            AssetRegulationViewerState.TestFilterType.Skip(1)
                .Subscribe(x => EditorPrefs.SetInt(TestFilterTypeKey, (int)x))
                .DisposeWith(_disposables);

            AssetRegulationViewerPresenter = new AssetRegulationViewerPresenter(store, AssetRegulationViewerState);
            AssetRegulationViewerController =
                new AssetRegulationViewerController(store, store, AssetRegulationViewerState);
        }

        public AssetRegulationViewerPresenter AssetRegulationViewerPresenter { get; }
        public AssetRegulationViewerController AssetRegulationViewerController { get; }
        private AssetRegulationViewerState AssetRegulationViewerState { get; }

        public void Dispose()
        {
            _disposables.Dispose();
            AssetRegulationViewerPresenter.Dispose();
            AssetRegulationViewerController.Dispose();
            AssetRegulationViewerState.Dispose();
        }

        public static AssetRegulationManagerApplication RequestInstance()
        {
            if (_referenceCount++ == 0) _instance = new AssetRegulationManagerApplication();

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

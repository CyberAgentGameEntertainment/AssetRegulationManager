// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Tool.Test.AssetRegulationViewer;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.Test
{
    internal sealed class AssetRegulationTestApplication : IDisposable
    {
        private const string TestFilterTypeKey = "TestFilterType";
        private static int _referenceCount;
        private static AssetRegulationTestApplication _instance;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        private AssetRegulationTestApplication()
        {
            var repository = new AssetRegulationRepository();
            var testStore = new AssetRegulationTestStore();

            AssetRegulationViewerState = new AssetRegulationViewerState();
            var filterType =
                (AssetRegulationTestStoreFilter)EditorPrefs.GetInt(TestFilterTypeKey,
                    (int)AssetRegulationTestStoreFilter.ExcludeEmptyTests);
            AssetRegulationViewerState.TestFilterType.Value = filterType;
            AssetRegulationViewerState.TestFilterType.Skip(1)
                .Subscribe(x => EditorPrefs.SetInt(TestFilterTypeKey, (int)x))
                .DisposeWith(_disposables);

            AssetRegulationViewerPresenter = new AssetRegulationViewerPresenter(testStore, AssetRegulationViewerState);
            AssetRegulationViewerController =
                new AssetRegulationViewerController(repository, testStore, AssetRegulationViewerState);
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

        public static AssetRegulationTestApplication RequestInstance()
        {
            if (_referenceCount++ == 0)
                _instance = new AssetRegulationTestApplication();

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

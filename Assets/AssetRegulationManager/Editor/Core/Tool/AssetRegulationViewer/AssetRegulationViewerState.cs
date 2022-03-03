// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationViewer
{
    public class AssetRegulationViewerState : IDisposable
    {
        public ObservableProperty<string> SelectedAssetPath { get; } = new ObservableProperty<string>();

        public ObservableProperty<AssetRegulationTestStoreFilter> TestFilterType { get; } =
            new ObservableProperty<AssetRegulationTestStoreFilter>();

        public void Dispose()
        {
            SelectedAssetPath?.Dispose();
            TestFilterType?.Dispose();
        }
    }
}

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
        public ObservableProperty<TestFilterType> TestFilterType { get; } = new ObservableProperty<TestFilterType>();

        public void Dispose()
        {
            SelectedAssetPath?.Dispose();
            TestFilterType?.Dispose();
        }
    }
}

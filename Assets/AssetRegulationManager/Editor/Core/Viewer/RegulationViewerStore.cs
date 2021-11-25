// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Foundation.Observable.ObservableProperty;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal class RegulationViewerStore
    {
        internal RegulationViewerStore(AssetRegulationCollection assetRegulationCollection)
        {
            AssetRegulationCollection = assetRegulationCollection;
        }

        internal AssetRegulationCollection AssetRegulationCollection { get; }
        internal IObservableProperty<TestCollection> TestCollection { get; } = new ObservableProperty<TestCollection>();
    }
}
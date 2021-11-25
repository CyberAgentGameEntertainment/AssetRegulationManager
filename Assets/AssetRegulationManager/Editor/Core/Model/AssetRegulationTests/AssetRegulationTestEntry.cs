// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.Observable.ObservableProperty;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTests
{
    public class AssetRegulationTestEntry
    {
        public AssetRegulationTestEntry(IAssetRegulationEntry entry)
        {
            Entry = entry;
        }

        public string Description => Entry.Explanation;

        public IAssetRegulationEntry Entry { get; }

        public ObservableProperty<AssetRegulationTestResultType> Status { get; } =
            new ObservableProperty<AssetRegulationTestResultType>(AssetRegulationTestResultType.None);

        public void Run(Object obj)
        {
            Status.Value = Entry.RunTest(obj)
                ? AssetRegulationTestResultType.Success
                : AssetRegulationTestResultType.Failed;
        }
    }
}
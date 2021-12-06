// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.Observable.ObservableProperty;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTests
{
    internal sealed class AssetRegulationTestEntry
    {
        internal AssetRegulationTestEntry(IAssetRegulationEntry entry, string id)
        {
            Entry = entry;
            Id = id;
        }

        internal string Id { get; }
        internal string Description => Entry.Description;
        
        internal IAssetRegulationEntry Entry { get; }

        internal ObservableProperty<AssetRegulationTestResultType> Status { get; } =
            new ObservableProperty<AssetRegulationTestResultType>(AssetRegulationTestResultType.None);

        internal void Run(Object obj)
        {
            Status.Value = Entry.RunTest(obj)
                ? AssetRegulationTestResultType.Success
                : AssetRegulationTestResultType.Failed;
        }
    }
}
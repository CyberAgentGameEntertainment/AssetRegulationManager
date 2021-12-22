// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.Observable.ObservableProperty;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTests
{
    public sealed class AssetRegulationTestEntry
    {
        public AssetRegulationTestEntry(IAssetRegulationEntry regulationEntry)
        {
            Id = Guid.NewGuid().ToString();
            RegulationEntry = regulationEntry;
        }

        public string Id { get; }
        public string Description => RegulationEntry.Description;
        public IAssetRegulationEntry RegulationEntry { get; }

        public ObservableProperty<AssetRegulationTestStatus> Status { get; } =
            new ObservableProperty<AssetRegulationTestStatus>(AssetRegulationTestStatus.None);

        public void Run(Object obj)
        {
            Status.Value = RegulationEntry.RunTest(obj)
                ? AssetRegulationTestStatus.Success
                : AssetRegulationTestStatus.Failed;
        }
    }
}

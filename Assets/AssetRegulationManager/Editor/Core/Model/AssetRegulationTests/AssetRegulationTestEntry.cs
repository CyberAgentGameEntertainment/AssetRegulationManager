// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTests
{
    public sealed class AssetRegulationTestEntry
    {
        private readonly ObservableProperty<AssetRegulationTestStatus> _status =
            new ObservableProperty<AssetRegulationTestStatus>(AssetRegulationTestStatus.None);

        public AssetRegulationTestEntry(IAssetLimitation limitation)
        {
            Id = Guid.NewGuid().ToString();
            Limitation = limitation;
        }

        public string Id { get; }
        public string Description => Limitation.GetDescription();
        public IAssetLimitation Limitation { get; }

        public IReadOnlyObservableProperty<AssetRegulationTestStatus> Status => _status;

        internal void Run(Object obj)
        {
            var success = Limitation.Check(obj);
            _status.Value = success ? AssetRegulationTestStatus.Success : AssetRegulationTestStatus.Failed;
        }

        internal void ClearStatus()
        {
            _status.Value = AssetRegulationTestStatus.None;
        }
    }
}

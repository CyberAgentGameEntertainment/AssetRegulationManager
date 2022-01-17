using System;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTestResults
{
    [Serializable]
    public sealed class AssetRegulationTestEntryResult
    {
        public AssetRegulationTestStatus status;
        public string description;
    }
}

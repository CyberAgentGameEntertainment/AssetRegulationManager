using System;
using System.Collections.Generic;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTestResults
{
    [Serializable]
    public sealed class AssetRegulationTestResult
    {
        public string assetPath;
        public List<AssetRegulationTestEntryResult> entries = new List<AssetRegulationTestEntryResult>();

        public IEnumerable<string> GetAsTexts()
        {
            foreach (var entry in entries)
            {
                yield return $"[{entry.status.ToString()}] {assetPath} - {entry.description}";
            }
        }
    }
}

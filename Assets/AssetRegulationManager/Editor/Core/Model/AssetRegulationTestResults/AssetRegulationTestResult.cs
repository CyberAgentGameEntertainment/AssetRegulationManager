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
                var text = $"[{entry.status}] {assetPath}";
                if (!string.IsNullOrEmpty(entry.description))
                {
                    text += $"| {entry.description}";
                }
                if (!string.IsNullOrEmpty(entry.message))
                {
                    text += $" | {entry.message}";
                }

                yield return text;
            }
        }
    }
}

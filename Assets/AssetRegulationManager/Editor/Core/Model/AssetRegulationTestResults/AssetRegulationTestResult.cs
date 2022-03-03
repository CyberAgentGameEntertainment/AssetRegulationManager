using System;
using System.Collections.Generic;
using System.Text;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTestResults
{
    [Serializable]
    public sealed class AssetRegulationTestResult
    {
        public string assetPath;
        public string status;
        public List<AssetRegulationTestEntryResult> entries = new List<AssetRegulationTestEntryResult>();

        public string GetAsText()
        {
            var text = new StringBuilder(assetPath);
            foreach (var entry in entries)
            {
                text.Append(Environment.NewLine);
                text.Append($"  [{entry.status}] {entry.description}");
                
                if (!string.IsNullOrEmpty(entry.message))
                    text.Append($" | {entry.message}");
            }

            return text.ToString();
        }
    }
}

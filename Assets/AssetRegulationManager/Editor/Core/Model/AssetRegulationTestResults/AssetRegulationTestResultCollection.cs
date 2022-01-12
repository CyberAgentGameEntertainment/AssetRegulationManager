using System;
using System.Collections.Generic;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTestResults
{
    [Serializable]
    public sealed class AssetRegulationTestResultCollection
    {
        public List<AssetRegulationTestResult> results = new List<AssetRegulationTestResult>();
        
        public IEnumerable<string> GetAsTexts()
        {
            foreach (var result in results)
            {
                foreach (var text in result.GetAsTexts())
                {
                    yield return text;
                }
            }
        }
    }
}

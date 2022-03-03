using System;
using System.Collections.Generic;
using System.Text;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTestResults
{
    [Serializable]
    public sealed class AssetRegulationTestResultCollection
    {
        public List<AssetRegulationTestResult> results = new List<AssetRegulationTestResult>();

        public string GetAsText()
        {
            var resultText = new StringBuilder();
            foreach (var result in results)
            {
                if (resultText.Length >= 1)
                {
                    resultText.Append(Environment.NewLine);
                    resultText.Append(Environment.NewLine);
                }

                resultText.Append(result.GetAsText());
            }

            return resultText.ToString();
        }
    }
}

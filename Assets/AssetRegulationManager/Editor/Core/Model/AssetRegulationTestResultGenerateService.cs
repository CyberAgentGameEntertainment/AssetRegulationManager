using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTestResults;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;

namespace AssetRegulationManager.Editor.Core.Model
{
    public sealed class AssetRegulationTestResultGenerateService
    {
        private readonly IAssetRegulationTestStore _store;

        public AssetRegulationTestResultGenerateService(IAssetRegulationTestStore store)
        {
            _store = store;
        }

        public AssetRegulationTestResultCollection Run(IList<AssetRegulationTestStatus> targetStatus = null)
        {
            var resultCollection = new AssetRegulationTestResultCollection();
            foreach (var test in _store.Tests.Values)
            {
                if (test.Entries.Count == 0)
                {
                    continue;
                }

                var result = CreateResultFromTest(test, targetStatus);

                if (result.entries.Count == 0)
                {
                    continue;
                }

                resultCollection.results.Add(result);
            }

            return resultCollection;
        }

        private static AssetRegulationTestResult CreateResultFromTest(AssetRegulationTest test,
            IList<AssetRegulationTestStatus> targetStatus = null)
        {
            if (targetStatus == null)
            {
                targetStatus = new List<AssetRegulationTestStatus>
                {
                    AssetRegulationTestStatus.Success,
                    AssetRegulationTestStatus.Failed
                };
            }
            
            var result = new AssetRegulationTestResult();
            result.assetPath = test.AssetPath;
            foreach (var entry in test.Entries.Values)
            {
                if (!targetStatus.Contains(entry.Status.Value))
                {
                    continue;
                }

                var entryResult = new AssetRegulationTestEntryResult();
                entryResult.description = entry.Description;
                entryResult.status = entry.Status.Value;
                result.entries.Add(entryResult);
            }

            return result;
        }
    }
}

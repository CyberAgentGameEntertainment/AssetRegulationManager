using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTestResults;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model
{
    public sealed class AssetRegulationTestResultGenerateService
    {
        private readonly IAssetRegulationTestStore _store;

        public AssetRegulationTestResultGenerateService(IAssetRegulationTestStore store)
        {
            _store = store;
        }

        public AssetRegulationTestResultCollection Run(IReadOnlyList<AssetRegulationTestStatus> targetStatusList = null)
        {
            var resultCollection = new AssetRegulationTestResultCollection();
            foreach (var test in _store.GetTests())
            {
                if (_store.ExcludeEmptyTests.Value && test.Entries.Count == 0)
                {
                    continue;
                }

                var result = CreateResultFromTest(test, _store.ExcludeEmptyTests.Value, targetStatusList);

                if (_store.ExcludeEmptyTests.Value && result.entries.Count == 0)
                {
                    continue;
                }

                resultCollection.results.Add(result);
            }
            
            return resultCollection;
        }

        private static AssetRegulationTestResult CreateResultFromTest(AssetRegulationTest test, bool excludeEmptyTests,
            IReadOnlyList<AssetRegulationTestStatus> targetStatusList = null)
        {
            if (targetStatusList == null)
            {
                var statusList = new List<AssetRegulationTestStatus>();
                foreach (AssetRegulationTestStatus status in Enum.GetValues(typeof(AssetRegulationTestStatus)))
                {
                    if (status == AssetRegulationTestStatus.None)
                    {
                        continue;
                    }

                    statusList.Add(status);
                }

                targetStatusList = statusList;
            }
            
            var result = new AssetRegulationTestResult();
            result.assetPath = test.AssetPath;

            if (!excludeEmptyTests && !test.Entries.Values.Any())
            {
                if (test.LatestStatus.Value == AssetRegulationTestStatus.None || !targetStatusList.Contains(test.LatestStatus.Value))
                {
                    return result;
                }
                var entryResult = new AssetRegulationTestEntryResult();
                entryResult.status = AssetRegulationTestStatus.Success.ToString();

                result.entries.Add(entryResult);

                return result; 
            }

            foreach (var entry in test.Entries.Values)
            {
                if (!targetStatusList.Contains(entry.Status.Value))
                {
                    continue;
                }

                var entryResult = new AssetRegulationTestEntryResult();
                entryResult.description = entry.Description;
                entryResult.status = entry.Status.Value.ToString();
                entryResult.message = entry.Message.Value;
                result.entries.Add(entryResult);
            }

            return result;
        }
    }
}

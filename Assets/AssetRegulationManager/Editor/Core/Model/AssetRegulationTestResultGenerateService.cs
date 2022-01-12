using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTestResults;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;

namespace AssetRegulationManager.Editor.Core.Model
{
    public sealed class AssetRegulationTestResultGenerateService
    {
        private readonly AssetRegulationManagerStore _store;

        public AssetRegulationTestResultGenerateService(AssetRegulationManagerStore store)
        {
            _store = store;
        }

        public AssetRegulationTestResultCollection Run()
        {
            var result = new AssetRegulationTestResultCollection();
            foreach (var test in _store.Tests.Values)
            {
                result.results.Add(CreateResultFromTest(test));
            }

            return result;
        }

        private static AssetRegulationTestResult CreateResultFromTest(AssetRegulationTest test)
        {
            var result = new AssetRegulationTestResult();
            result.assetPath = test.AssetPath;
            foreach (var entry in test.Entries.Values)
            {
                var entryResult = new AssetRegulationTestEntryResult();
                entryResult.description = entry.Description;
                entryResult.status = entry.Status.Value;
                result.entries.Add(entryResult);
            }

            return result;
        }
    }
}

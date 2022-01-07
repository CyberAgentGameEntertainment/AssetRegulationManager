// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.Adapters;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;

namespace AssetRegulationManager.Editor.Core.Model
{
    public sealed class AssetRegulationTestGenerateService
    {
        private readonly IAssetDatabaseAdapter _assetDatabaseAdapter;
        private readonly AssetRegulationManagerStore _store;

        public AssetRegulationTestGenerateService(AssetRegulationManagerStore store,
            IAssetDatabaseAdapter assetDatabaseAdapter)
        {
            _store = store;
            _assetDatabaseAdapter = assetDatabaseAdapter;
        }

        public void Run(string assetPathOrFilter)
        {
            var assetPaths = _assetDatabaseAdapter.FindAssetPaths(assetPathOrFilter).ToArray();

            _store.ClearTests();

            if (string.IsNullOrEmpty(assetPathOrFilter))
            {
                return;
            }

            var regulations = _store.GetRegulations().ToArray();

            // Setup all AssetGroups.
            foreach (var regulation in regulations)
            {
                regulation.AssetGroup.Setup();
            }

            // Grouping by 100 AssetPaths.
            var assetPathGroups = assetPaths.Select((v, i) => new { v, i })
                .GroupBy(x => x.i / 100)
                .Select(g => g.Select(x => x.v));

            // Process each group in different threads.
            var createTestsTasks = assetPathGroups
                .Select(assetPathGroup =>
                    CreateTestsAsync(assetPathGroup.ToArray(), regulations, _assetDatabaseAdapter))
                .ToList();

            var tests = Task.WhenAll(createTestsTasks).Result;
            foreach (var test in tests)
            {
                _store.AddTests(test);
            }
        }

        private static Task<AssetRegulationTest[]> CreateTestsAsync(IList<string> assetPaths,
            IList<AssetRegulation> regulations, IAssetDatabaseAdapter assetDatabaseAdapter)
        {
            return Task.Run(() =>
            {
                var result = new AssetRegulationTest[assetPaths.Count];
                for (var i = 0; i < assetPaths.Count; i++)
                {
                    var assetPath = assetPaths[i];
                    var test = new AssetRegulationTest(assetPath, assetDatabaseAdapter);
                    foreach (var regulation in regulations)
                    {
                        if (!regulation.AssetGroup.Contains(assetPath))
                        {
                            continue;
                        }

                        foreach (var limitation in regulation.AssetSpec.Limitations)
                        {
                            test.AddLimitation(limitation);
                        }
                    }

                    result[i] = test;
                }

                return result;
            });
        }
    }
}

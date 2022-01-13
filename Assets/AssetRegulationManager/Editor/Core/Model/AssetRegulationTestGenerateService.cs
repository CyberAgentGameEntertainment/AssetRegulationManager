// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        private readonly IAssetRegulationStore _regulationStore;
        private readonly IAssetRegulationTestStore _testStore;

        public AssetRegulationTestGenerateService(IAssetRegulationStore regulationStore,
            IAssetRegulationTestStore testStore, IAssetDatabaseAdapter assetDatabaseAdapter)
        {
            _regulationStore = regulationStore;
            _testStore = testStore;
            _assetDatabaseAdapter = assetDatabaseAdapter;
        }

        /// <summary>
        ///     Generate the asset regulation tests.
        /// </summary>
        /// <param name="assetFilter">Similar to what you enter in the search field of the project view.</param>
        /// <param name="excludeEmptyTests">If true, exclude tests that do not contain any entries.</param>
        /// <param name="regulationDescriptionFilters">
        ///     If not empty, only regulations whose description matches this regex will be
        ///     considered.
        /// </param>
        public void Run(string assetFilter, bool excludeEmptyTests,
            IList<string> regulationDescriptionFilters = null)
        {
            var assetPaths = string.IsNullOrWhiteSpace(assetFilter)
                ? Array.Empty<string>()
                : _assetDatabaseAdapter.FindAssetPaths(assetFilter);

            RunInternal(assetPaths, excludeEmptyTests, regulationDescriptionFilters);
        }

        /// <summary>
        ///     Generate the asset regulation tests.
        /// </summary>
        /// <param name="assetPathFilters">Only assets whose name matches this regex will be considered.</param>
        /// <param name="excludeEmptyTests">If true, exclude tests that do not contain any entries.</param>
        /// <param name="regulationDescriptionFilters">
        ///     If not empty, only regulations whose description matches this regex will be
        ///     considered.
        /// </param>
        public void Run(IEnumerable<string> assetPathFilters, bool excludeEmptyTests,
            IList<string> regulationDescriptionFilters = null)
        {
            var assetPathFilterRegexes = assetPathFilters.Select(x => new Regex(x)).ToArray();

            // Grouping by 100 AssetPaths.
            var assetPaths = _assetDatabaseAdapter.GetAllAssetPaths();
            var assetPathGroups = assetPaths.Select((v, i) => new { v, i })
                .GroupBy(x => x.i / 100)
                .Select(g => g.Select(x => x.v).ToArray());

            // Process each group in different threads.
            var matchedAssetPathsTasks = assetPathGroups
                .Select(assetPathGroup => GetMatchedAssetPathsAsync(assetPathGroup, assetPathFilterRegexes))
                .ToList();

            var matchedAssetPaths = Task.WhenAll(matchedAssetPathsTasks).Result.SelectMany(x => x);

            RunInternal(matchedAssetPaths, excludeEmptyTests, regulationDescriptionFilters);
        }

        private void RunInternal(IEnumerable<string> assetPaths, bool excludeEmptyTests,
            IList<string> regulationDescriptionFilters = null)
        {
            _testStore.ClearTests();

            var regulations = _regulationStore.GetRegulations().ToArray();

            if (regulationDescriptionFilters != null)
            {
                var regulationDescriptionFiltersRegexes = regulationDescriptionFilters
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => new Regex(x));
                regulations = regulations
                    .Where(x => regulationDescriptionFiltersRegexes.Any(y => y.IsMatch(x.Description)))
                    .ToArray();
            }

            // Setup all AssetGroups.
            foreach (var regulation in regulations)
            {
                regulation.AssetGroup.Setup();
            }

            // Grouping by 100 AssetPaths.
            var assetPathGroups = assetPaths.Select((v, i) => new { v, i })
                .GroupBy(x => x.i / 100)
                .Select(g => g.Select(x => x.v).ToArray());

            // Process each group in different threads.
            var createTestsTasks = assetPathGroups
                .Select(assetPathGroup =>
                    CreateTestsAsync(assetPathGroup, regulations, _assetDatabaseAdapter, excludeEmptyTests))
                .ToList();

            var tests = Task.WhenAll(createTestsTasks).Result;
            foreach (var test in tests)
            {
                _testStore.AddTests(test);
            }
        }

        private static Task<string[]> GetMatchedAssetPathsAsync(IList<string> assetPaths,
            Regex[] assetPathFilterRegexes)
        {
            return Task.Run(() =>
            {
                return assetPaths.Where(x => { return assetPathFilterRegexes.Any(y => y.IsMatch(x)); }).ToArray();
            });
        }

        private static Task<AssetRegulationTest[]> CreateTestsAsync(IList<string> assetPaths,
            IList<AssetRegulation> regulations, IAssetDatabaseAdapter assetDatabaseAdapter, bool stripEmptyTests)
        {
            return Task.Run(() =>
            {
                var result = new List<AssetRegulationTest>();
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
                            test.AddEntry(limitation);
                        }
                    }

                    if (stripEmptyTests && test.Entries.Count == 0)
                    {
                        continue;
                    }

                    result.Add(test);
                }

                return result.ToArray();
            });
        }
    }
}

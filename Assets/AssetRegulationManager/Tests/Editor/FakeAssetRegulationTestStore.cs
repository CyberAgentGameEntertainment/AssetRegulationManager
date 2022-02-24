using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;

namespace AssetRegulationManager.Tests.Editor
{
    internal sealed class FakeAssetRegulationTestStore : IAssetRegulationTestStore
    {
        private readonly ObservableDictionary<string, AssetRegulationTest> _tests =
            new ObservableDictionary<string, AssetRegulationTest>();

        private readonly ObservableList<AssetRegulationTest> _filteredTests = new ObservableList<AssetRegulationTest>();

        public IReadOnlyObservableDictionary<string, AssetRegulationTest> Tests => _tests;

        public IReadOnlyObservableList<AssetRegulationTest> FilteredTests => _filteredTests;
        
        public void FilterTests(TestFilterType testFilterType)
        {
            _filteredTests.Clear();

            foreach (var test in GetFilteredTests(testFilterType))
                _filteredTests.Add(test);
        }

        private IEnumerable<AssetRegulationTest> GetFilteredTests(TestFilterType testSortType)
        {
            switch (testSortType)
            {
                case TestFilterType.All:
                    return Tests.Values;
                case TestFilterType.ExcludeEmptyTests:
                    return Tests.Values.Where(test => test.Entries.Any());
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddTests(IEnumerable<AssetRegulationTest> tests)
        {
            foreach (var test in tests)
            {
                _tests.Add(test.Id, test);
            }
        }

        public void ClearTests()
        {
            _tests.Clear();
        }
    }
}

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
        private readonly ObservableList<AssetRegulationTest> _filteredTests = new ObservableList<AssetRegulationTest>();

        private readonly ObservableDictionary<string, AssetRegulationTest> _tests =
            new ObservableDictionary<string, AssetRegulationTest>();

        public IReadOnlyObservableDictionary<string, AssetRegulationTest> Tests => _tests;

        public AssetRegulationTestStoreFilter Filter { get; private set; } = AssetRegulationTestStoreFilter.All;

        public IReadOnlyObservableList<AssetRegulationTest> FilteredTests => _filteredTests;

        public void FilterTests(AssetRegulationTestStoreFilter filter)
        {
            _filteredTests.Clear();

            foreach (var test in GetFilteredTests(filter))
                _filteredTests.Add(test);

            Filter = filter;
        }

        public void AddTests(IEnumerable<AssetRegulationTest> tests, bool doFilterAfterAdd)
        {
            foreach (var test in tests)
                _tests.Add(test.Id, test);

            if (doFilterAfterAdd)
                FilterTests(Filter);
        }

        public void ClearTests()
        {
            _tests.Clear();
        }

        private IEnumerable<AssetRegulationTest> GetFilteredTests(AssetRegulationTestStoreFilter filter)
        {
            switch (filter)
            {
                case AssetRegulationTestStoreFilter.All:
                    return Tests.Values;
                case AssetRegulationTestStoreFilter.ExcludeEmptyTests:
                    return Tests.Values.Where(test => test.Entries.Any());
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

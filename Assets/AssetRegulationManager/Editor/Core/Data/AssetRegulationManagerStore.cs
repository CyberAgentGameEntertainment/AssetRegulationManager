// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;

namespace AssetRegulationManager.Editor.Core.Data
{
    /// <summary>
    ///     Class to store the data of this application in memory.
    /// </summary>
    public sealed class AssetRegulationManagerStore : IAssetRegulationStore, IAssetRegulationTestStore
    {
        private readonly IAssetRegulationRepository _repository;

        public AssetRegulationManagerStore(IAssetRegulationRepository repository)
        {
            _repository = repository;
        }

        private ObservableDictionary<string, AssetRegulationTest> _tests { get; } =
            new ObservableDictionary<string, AssetRegulationTest>();

        private readonly ObservableList<AssetRegulationTest> _filteredTests = new ObservableList<AssetRegulationTest>();

        public IReadOnlyObservableDictionary<string, AssetRegulationTest> Tests => _tests;

        public AssetRegulationTestStoreFilter Filter { get; private set; } = AssetRegulationTestStoreFilter.All;

        public IReadOnlyObservableList<AssetRegulationTest> FilteredTests => _filteredTests;

        public IEnumerable<AssetRegulation> GetRegulations()
        {
            return _repository.GetAllRegulations();
        }

        public void FilterTests(AssetRegulationTestStoreFilter filter)
        {
            _filteredTests.Clear();

            foreach (var test in GetFilteredTests(filter))
                _filteredTests.Add(test);

            Filter = filter;
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

        void IAssetRegulationTestStore.AddTests(IEnumerable<AssetRegulationTest> tests)
        {
            foreach (var test in tests)
            {
                _tests.Add(test.Id, test);
            }

            FilterTests(Filter);
        }

        void IAssetRegulationTestStore.ClearTests()
        {
            _tests.Clear();
        }
    }
}

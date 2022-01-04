// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.Observable.ObservableCollection;

namespace AssetRegulationManager.Editor.Core.Data
{
    /// <summary>
    ///     Class to store the data of this application in memory.
    /// </summary>
    public sealed class AssetRegulationManagerStore
    {
        private readonly IAssetRegulationRepository _repository;

        public AssetRegulationManagerStore(IAssetRegulationRepository repository)
        {
            _repository = repository;
        }

        private ObservableDictionary<string, AssetRegulationTest> _tests { get; } =
            new ObservableDictionary<string, AssetRegulationTest>();

        public IReadOnlyObservableDictionary<string, AssetRegulationTest> Tests => _tests;

        public IEnumerable<AssetRegulation> GetRegulations()
        {
            return _repository.GetAllRegulations();
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

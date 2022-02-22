// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;

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

        public IReadOnlyObservableDictionary<string, AssetRegulationTest> Tests => _tests;

        public IEnumerable<AssetRegulation> GetRegulations()
        {
            return _repository.GetAllRegulations();
        }

        void IAssetRegulationTestStore.AddTests(IEnumerable<AssetRegulationTest> tests)
        {
            foreach (var test in tests)
            {
                _tests.Add(test.Id, test);
            }
        }

        void IAssetRegulationTestStore.ClearTests()
        {
            _tests.Clear();
        }
    }
}

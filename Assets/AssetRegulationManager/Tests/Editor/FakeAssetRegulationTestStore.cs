using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;

namespace AssetRegulationManager.Tests.Editor
{
    internal sealed class FakeAssetRegulationTestStore : IAssetRegulationTestStore
    {
        private readonly ObservableDictionary<string, AssetRegulationTest> _tests =
            new ObservableDictionary<string, AssetRegulationTest>();

        public IReadOnlyObservableDictionary<string, AssetRegulationTest> Tests => _tests;

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

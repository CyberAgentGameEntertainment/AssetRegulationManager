using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;

namespace AssetRegulationManager.Tests.Editor
{
    internal sealed class FakeAssetRegulationTestStore : IAssetRegulationTestStore
    {
        private readonly ObservableDictionary<string, AssetRegulationTest> _tests =
            new ObservableDictionary<string, AssetRegulationTest>();

        public IReadOnlyObservableDictionary<string, AssetRegulationTest> Tests => _tests;
        public BoolObservableProperty ExcludeEmptyTests { get; } = new BoolObservableProperty();

        public IReadOnlyCollection<AssetRegulationTest> GetTests(bool excludeEmptyTests)
        {
            return _tests.Values.Where(test => !excludeEmptyTests || test.Entries.Any()).ToList();
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

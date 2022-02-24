using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;

namespace AssetRegulationManager.Editor.Core.Data
{
    public interface IAssetRegulationTestStore
    {
        IReadOnlyObservableDictionary<string, AssetRegulationTest> Tests { get; }
        
        IReadOnlyObservableList<AssetRegulationTest> SortedTests { get; }
        
        void SortTests(TestSortType testSortType);

        void AddTests(IEnumerable<AssetRegulationTest> tests);

        void ClearTests();
    }
}

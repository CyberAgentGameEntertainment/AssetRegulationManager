using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;

namespace AssetRegulationManager.Editor.Core.Data
{
    public interface IAssetRegulationTestStore
    {
        IReadOnlyObservableDictionary<string, AssetRegulationTest> Tests { get; }
        
        IReadOnlyObservableList<AssetRegulationTest> FilteredTests { get; }

        void FilterTests(AssetRegulationTestFilterType testFilterType);

        void AddTests(IEnumerable<AssetRegulationTest> tests);

        void ClearTests();
    }
}

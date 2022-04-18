using System;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    [Serializable]
    public sealed class AssetFilterObservableDictionary : SerializeReferenceObservableDictionary<string, IAssetFilter>
    {
    }
}

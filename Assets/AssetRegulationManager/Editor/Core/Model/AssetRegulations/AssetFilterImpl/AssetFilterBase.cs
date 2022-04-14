using System;
using AssetRegulationManager.Editor.Core.Shared;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl
{
    [Serializable]
    public abstract class AssetFilterBase : IAssetFilter
    {
        [SerializeField] private string _id;

        protected AssetFilterBase()
        {
            _id = IdentifierFactory.Create();
        }

        public string Id => _id;
        public abstract void SetupForMatching();

        public abstract bool IsMatch(string assetPath, Type assetType);

        public abstract string GetDescription();
    }
}

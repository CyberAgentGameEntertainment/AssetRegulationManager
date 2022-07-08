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
        
        /// <inheritdoc/>
        public abstract void SetupForMatching();

        /// <inheritdoc/>
        public abstract bool IsMatch(string assetPath, Type assetType, bool isFolder);

        /// <inheritdoc/>
        public abstract string GetDescription();

        public void OverwriteValuesFromJson(string json)
        {
            var id = _id;
            JsonUtility.FromJsonOverwrite(json, this);
            _id = id;
        }
    }
}

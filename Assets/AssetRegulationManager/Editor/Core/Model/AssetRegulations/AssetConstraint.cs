// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Core.Shared;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    [Serializable]
    public abstract class AssetConstraint<TAsset> : IAssetConstraint where TAsset : Object
    {
        [SerializeField] private string _id;

        protected AssetConstraint()
        {
            _id = IdentifierFactory.Create();
        }

        public string Id => _id;

        public bool Check(Object asset)
        {
            return CheckInternal((TAsset)asset);
        }

        /// <inheritdoc/>
        public abstract string GetDescription();

        /// <inheritdoc/>
        public abstract string GetLatestValueAsText();

        public void OverwriteValuesFromJson(string json)
        {
            var id = _id;
            JsonUtility.FromJsonOverwrite(json, this);
            _id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        protected abstract bool CheckInternal(TAsset asset);
    }
}

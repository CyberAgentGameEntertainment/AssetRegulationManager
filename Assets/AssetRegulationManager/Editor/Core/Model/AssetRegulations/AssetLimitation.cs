// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    public abstract class AssetLimitation<TAsset> : IAssetLimitation where TAsset : Object
    {
        public bool Check(Object asset)
        {
            return CheckInternal((TAsset)asset);
        }

        public abstract string GetDescription();

        public abstract string GetLatestValueAsText();

        /// <inheritdocs />
        protected abstract bool CheckInternal(TAsset asset);
    }
}

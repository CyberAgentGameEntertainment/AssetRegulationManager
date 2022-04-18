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

        public abstract string GetDescription();

        public abstract string GetLatestValueAsText();

        /// <inheritdocs />
        protected abstract bool CheckInternal(TAsset asset);
    }
}

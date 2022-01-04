// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    /// <summary>
    ///     Define regulations for assets.
    /// </summary>
    [Serializable]
    public sealed class AssetRegulation
    {
        [SerializeField] private string _description;
        [SerializeField] private AssetGroup _assetGroup = new AssetGroup();
        [SerializeField] private AssetSpecification _assetSpec = new AssetSpecification();

        public string Description
        {
            set => _description = value;
            get => _description;
        }

        /// <summary>
        /// </summary>
        public AssetGroup AssetGroup => _assetGroup;

        public AssetSpecification AssetSpec => _assetSpec;
    }
}

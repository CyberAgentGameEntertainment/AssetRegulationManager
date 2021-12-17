// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Data
{
    /// <summary>
    ///     Route of regulation data.
    /// </summary>
    [CreateAssetMenu]
    public sealed class AssetRegulationSettings : ScriptableObject
    {
        [SerializeField] private List<AssetRegulation> _regulations = new List<AssetRegulation>();

        /// <summary>
        ///     Regulation Collection.
        /// </summary>
        public List<AssetRegulation> Regulations => _regulations;
    }
}
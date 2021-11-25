// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core
{
    /// <summary>
    ///     Route of regulation data.
    /// </summary>
    public sealed class AssetRegulationCollection : ScriptableObject
    {
        [SerializeField] private List<AssetRegulation> _regulations = new List<AssetRegulation>();

        /// <summary>
        ///     Regulation Collection.
        /// </summary>
        public List<AssetRegulation> Regulations => _regulations;
    }
}
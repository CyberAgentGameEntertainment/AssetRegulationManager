// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using AssetRegulationManager.Editor.Foundation.SelectableSerializeReference;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    /// <summary>
    ///     AssetRegulation data class.
    /// </summary>
    [Serializable]
    public class AssetRegulation
    {
        [SerializeField] private string _name;
        [SerializeField] private string _assetPathRegex;

        [SerializeReference, SelectableSerializeReference]
        private List<IAssetRegulationEntry> _entries = new List<IAssetRegulationEntry>();

        /// <summary>
        ///     Regulation Name.
        /// </summary>
        public string Name
        {
            set => _name = value;
            get => _name;
        }

        /// <summary>
        ///     Regex for paths
        /// </summary>
        public string AssetPathRegex
        {
            set => _assetPathRegex = value;
            get => _assetPathRegex;
        }

        public List<IAssetRegulationEntry> Entries => _entries;
    }
}
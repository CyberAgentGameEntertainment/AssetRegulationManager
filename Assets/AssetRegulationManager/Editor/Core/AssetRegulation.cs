// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core
{
    /// <summary>
    ///     AssetRegulation data class.
    /// </summary>
    [Serializable]
    public class AssetRegulation
    {
        [SerializeField] private string _name;
        [SerializeField] private string _assetPathRegex;
        [SerializeReference] private List<IAssetRegulationEntry> _entries;

        /// <summary>
        ///     Initialize.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name "></param>
        /// <param name="assetPathRegex"></param>
        /// <param name="entries"></param>
        public AssetRegulation(string id, string name, string assetPathRegex, List<IAssetRegulationEntry> entries)
        {
            Id = id;
            _name = name;
            _assetPathRegex = assetPathRegex;
            _entries = entries;
        }
        
        /// <summary>
        ///     Regulation management ID.
        /// </summary>
        public string Id { get; }
        
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
        
        /// <summary>
        ///     Regulation Entry Collection.
        /// </summary>
        public List<IAssetRegulationEntry> Entries => _entries;
    }
}
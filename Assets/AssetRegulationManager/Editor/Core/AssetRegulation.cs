// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core
{
    [Serializable]
    public class AssetRegulation
    {
        [SerializeField] private string _name;
        [SerializeField] private string _assetPathRegex;
        [SerializeReference] private List<IAssetRegulationEntry> _entries;

        private string _id;

        public string Id => _id;
        public string Name
        {
            set => _name = value;
            get => _name;
        }
        public string AssetPathRegex
        {
            set => _assetPathRegex = value;
            get => _assetPathRegex;
        }
        public List<IAssetRegulationEntry> Entries => _entries;
        
        public AssetRegulation(string id, string name, string assetPathRegex, List<IAssetRegulationEntry> entries)
        {
            _id = id;
            _name = name;
            _assetPathRegex = assetPathRegex;
            _entries = entries;
        }
    }
}
// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace AssetRegulationManager.Editor
{
    public sealed class AssetRegulationCollection : ScriptableObject
    {
        [SerializeField] private List<AssetRegulation> _regulations = new List<AssetRegulation>();

        public List<AssetRegulation> Regulations => _regulations;
    }
}
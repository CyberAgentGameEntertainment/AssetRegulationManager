using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRegulationManager.Editor
{
    [Serializable]
    public class AssetRegulation
    {
        public int id;
        public string name;
        public string assetPathRegex;
        [SerializeReference] public List<IAssetRegulationEntry> regulations;
    }
}
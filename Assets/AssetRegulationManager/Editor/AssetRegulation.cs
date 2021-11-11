using System;
using System.Collections.Generic;

namespace AssetRegulationManager.Editor
{
    [Serializable]
    public class AssetRegulation
    {
        public int id;
        public string name;
        public string assetPathRegex;
        public List<IAssetRegulationEntry> regulations;
    }
}
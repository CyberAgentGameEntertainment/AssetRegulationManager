using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Data
{
    [CreateAssetMenu(fileName = "Asset Regulation Data", menuName = "Asset Regulation Data")]
    public sealed class AssetRegulationSetStore : ScriptableObject
    {
        [SerializeField] private AssetRegulationSet _set = new AssetRegulationSet();

        public AssetRegulationSet Set => _set;
    }
}

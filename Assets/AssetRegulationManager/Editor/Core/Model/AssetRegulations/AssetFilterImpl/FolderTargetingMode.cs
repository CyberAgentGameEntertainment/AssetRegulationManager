using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl
{
    public enum FolderTargetingMode
    {
        [InspectorName("Included Assets (Exclude Folders)")]
        IncludedNonFolderAssets = 0,
        Self,
        Both,
    }
}

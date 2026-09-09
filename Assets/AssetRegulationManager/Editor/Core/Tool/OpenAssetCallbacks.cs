using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_6000_5_OR_NEWER
using UnityEngine;
#endif

namespace AssetRegulationManager.Editor.Core.Tool
{
    public sealed class OpenAssetCallback
    {
        [OnOpenAsset(0)]
#if UNITY_6000_5_OR_NEWER
        public static bool OnOpen(EntityId instanceID, int line)
        {
            var asset = EditorUtility.EntityIdToObject(instanceID);
#else
        public static bool OnOpen(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID);
#endif

            if (asset is AssetRegulationSetStore store)
            {
                AssetRegulationEditorWindow.Open(store);
                return true;
            }

            return false;
        }
    }
}

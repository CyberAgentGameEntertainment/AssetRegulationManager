using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor;
using UnityEditor;
using UnityEditor.Callbacks;

namespace AssetRegulationManager.Editor.Core.Tool
{
    public sealed class OpenAssetCallback
    {
        [OnOpenAsset(0)]
        public static bool OnOpen(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID);

            if (asset is AssetRegulationSetStore store)
            {
                AssetRegulationEditorWindow.Open(store);
                return true;
            }

            return false;
        }
    }
}

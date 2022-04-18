using AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Data
{
    [CustomEditor(typeof(AssetRegulationSetStore))]
    internal sealed class AssetRegulationSetStoreEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                var store = (AssetRegulationSetStore)target;
                AssetRegulationEditorWindow.Open(store);
            }

            GUI.enabled = false;
            base.OnInspectorGUI();
            GUI.enabled = true;
        }
    }
}

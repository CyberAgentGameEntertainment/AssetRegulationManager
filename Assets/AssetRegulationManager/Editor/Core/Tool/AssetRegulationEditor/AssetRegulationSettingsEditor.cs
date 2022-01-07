// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Data;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    [CustomEditor(typeof(AssetRegulationSettings))]
    internal sealed class AssetRegulationSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                var settings = (AssetRegulationSettings)target;
                AssetRegulationEditorWindow.Open(settings);
            }
        }
    }
}

using System;
using AssetRegulationManager.Editor.Core.Data;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorEmptyPanel
    {
        public string Message { get; set; } =
            $"The specified {nameof(AssetRegulationSetStore)} cannot be found.{Environment.NewLine}Please reopen this window from the {nameof(AssetRegulationSetStore)} Inspector.";

        public void DoLayout()
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(Message);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }
}

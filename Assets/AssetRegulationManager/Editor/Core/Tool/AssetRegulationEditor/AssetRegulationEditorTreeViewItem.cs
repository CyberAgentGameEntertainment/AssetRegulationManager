// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorTreeViewItem : TreeViewItem
    {
        public AssetRegulationEditorTreeViewItem(AssetRegulation regulation, SerializedProperty property)
        {
            Regulation = regulation;
            Property = property;
        }

        public AssetRegulation Regulation { get; }
        public SerializedProperty Property { get; }
    }
}

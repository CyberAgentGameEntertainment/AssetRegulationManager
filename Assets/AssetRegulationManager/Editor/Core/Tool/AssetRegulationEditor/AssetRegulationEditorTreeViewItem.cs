// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;
using UnityEditor.IMGUI.Controls;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorTreeViewItem : TreeViewItem
    {
        private const string DefaultName = "New Asset Regulation";

        public AssetRegulationEditorTreeViewItem(AssetRegulation regulation)
        {
            Regulation = regulation;
            var name = Regulation.Description;
            Name = new ObservableProperty<string>(name);
            displayName = GetRegulationName(name);
        }

        public AssetRegulation Regulation { get; }
        
        public ObservableProperty<string> Name { get; }

        public void SetName(string name)
        {
            Name.Value = name;
            displayName = GetRegulationName(name);
        }

        private static string GetRegulationName(string description)
        {
            return string.IsNullOrEmpty(description) ? DefaultName : description;
        }
    }
}

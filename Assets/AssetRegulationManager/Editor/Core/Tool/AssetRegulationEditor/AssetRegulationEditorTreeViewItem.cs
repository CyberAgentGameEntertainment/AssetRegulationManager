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
        public AssetRegulationEditorTreeViewItem(AssetRegulation regulation)
        {
            Regulation = regulation;
            Name = new ObservableProperty<string>(Regulation.Description);
        }

        public AssetRegulation Regulation { get; }
        
        public ObservableProperty<string> Name { get; }
    }
}

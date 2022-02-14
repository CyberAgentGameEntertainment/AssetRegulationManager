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
            SetName(name, false);
        }

        public AssetRegulation Regulation { get; }

        public ObservableProperty<string> Name { get; } = new ObservableProperty<string>();

        public void SetName(string name, bool notify)
        {
            if (notify)
                Name.Value = name;
            else
                Name.SetValueAndNotNotify(name);
            displayName = GetRegulationName(name);
        }

        private static string GetRegulationName(string description)
        {
            return string.IsNullOrEmpty(description) ? DefaultName : description;
        }
    }
}

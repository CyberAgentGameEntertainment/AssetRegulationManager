// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;
using UnityEditor.IMGUI.Controls;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorTreeViewItem : TreeViewItem
    {
        private readonly ObservableProperty<string> _name = new ObservableProperty<string>();

        public AssetRegulationEditorTreeViewItem(string regulationId)
        {
            RegulationId = regulationId;
        }

        public string RegulationId { get; }

        public IReadOnlyObservableProperty<string> Name => _name;

        public string TargetsDescription { get; set; }

        public string ConstraintsDescription { get; set; }

        public void SetName(string name, bool notify = true)
        {
            if (notify)
                _name.Value = name;
            else
                _name.SetValueAndNotNotify(name);

            displayName = name;
        }
    }
}

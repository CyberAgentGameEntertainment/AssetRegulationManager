// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;
using UnityEditor.IMGUI.Controls;
#if UNITY_6000_5_OR_NEWER
using TreeViewItemT = UnityEditor.IMGUI.Controls.TreeViewItem<int>;
#else
using TreeViewItemT = UnityEditor.IMGUI.Controls.TreeViewItem;
#endif

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorTreeViewItem : TreeViewItemT
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

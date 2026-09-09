// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using UnityEditor.IMGUI.Controls;
#if UNITY_6000_5_OR_NEWER
using TreeViewItemT = UnityEditor.IMGUI.Controls.TreeViewItem<int>;
#else
using TreeViewItemT = UnityEditor.IMGUI.Controls.TreeViewItem;
#endif

namespace AssetRegulationManager.Editor.Core.Tool.Test.AssetRegulationViewer
{
    internal sealed class AssetRegulationTestEntryTreeViewItem : TreeViewItemT
    {
        public AssetRegulationTestEntryTreeViewItem(string entryId, string explanation,
            AssetRegulationTestStatus status)
        {
            EntryId = entryId;
            Description = explanation;
            Status = status;
        }

        public string EntryId { get; }
        public string Description { get; }
        public string ActualValue { get; set; }
        public AssetRegulationTestStatus Status { get; set; }
    }
}

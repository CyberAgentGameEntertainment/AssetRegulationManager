// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using UnityEditor.IMGUI.Controls;

namespace AssetRegulationManager.Editor.Core.Tool.Test.AssetRegulationViewer
{
    internal sealed class AssetRegulationTestEntryTreeViewItem : TreeViewItem
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

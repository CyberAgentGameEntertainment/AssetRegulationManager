// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using UnityEditor.IMGUI.Controls;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class AssetRegulationTreeViewItem : TreeViewItem
    {
        internal AssetRegulationTreeViewItem(string entryId, string explanation,
            AssetRegulationTestResultType status)
        {
            EntryId = entryId;
            Description = explanation;
            Status = status;
        }
        
        internal string EntryId { get; }
        internal string Description { get; }
        internal AssetRegulationTestResultType Status { get; set; }
    }
}
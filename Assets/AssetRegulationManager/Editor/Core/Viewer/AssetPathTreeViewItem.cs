// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using UnityEditor.IMGUI.Controls;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class AssetPathTreeViewItem : TreeViewItem
    {
        internal AssetPathTreeViewItem(AssetRegulationTestResultType status)
        {
            Status = status;
        }
        
        internal AssetRegulationTestResultType Status { get; set; }
    }
}
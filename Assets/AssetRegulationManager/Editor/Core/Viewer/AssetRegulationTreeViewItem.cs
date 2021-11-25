// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using UnityEditor.IMGUI.Controls;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class AssetRegulationTreeViewItem : TreeViewItem
    {
        internal AssetRegulationTreeViewItem(string explanation,
            AssetRegulationTestResultType status)
        {
            Explanation = explanation;
            Status = status;
        }

        internal string Explanation { get; }
        internal AssetRegulationTestResultType Status { get; set; }
    }
}
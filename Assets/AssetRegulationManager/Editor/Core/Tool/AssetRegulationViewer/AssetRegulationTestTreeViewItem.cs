// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using UnityEditor.IMGUI.Controls;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationViewer
{
    internal sealed class AssetRegulationTestTreeViewItem : TreeViewItem
    {
        public AssetRegulationTestTreeViewItem(string testId, AssetRegulationTestStatus status)
        {
            TestId = testId;
            Status = status;
        }

        public string TestId { get; }

        public AssetRegulationTestStatus Status { get; set; }
    }
}

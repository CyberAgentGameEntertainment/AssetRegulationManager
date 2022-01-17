// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationViewer
{
    internal sealed class AssetRegulationTestTreeViewItem : TreeViewItem
    {
        public AssetRegulationTestTreeViewItem(string testId, AssetRegulationTestStatus status, Texture2D icon)
        {
            TestId = testId;
            Status = status;
            Icon = icon;
        }

        public string TestId { get; }
        public AssetRegulationTestStatus Status { get; set; }
        public Texture2D Icon { get; set; }
    }
}

// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.IO;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.Test.AssetRegulationViewer
{
    internal sealed class AssetRegulationTestTreeViewItem : TreeViewItem
    {
        private readonly string _assetPath;
        private string _displayName;
        private Texture2D _icon;

        public AssetRegulationTestTreeViewItem(string testId, AssetRegulationTestStatus status, string assetPath)
        {
            TestId = testId;
            Status = status;
            _assetPath = assetPath;
        }

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_displayName))
                    _displayName = Path.GetFileNameWithoutExtension(_assetPath);
                return _displayName;
            }
        }

        public string TestId { get; }
        public AssetRegulationTestStatus Status { get; set; }

        public Texture2D GetIcon()
        {
            if (_icon == null)
                _icon = (Texture2D)AssetDatabase.GetCachedIcon(_assetPath);

            return _icon;
        }
    }
}

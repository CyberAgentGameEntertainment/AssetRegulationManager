// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.EasyTreeView;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationViewer
{
    internal sealed class AssetRegulationViewerTreeView : TreeViewBase
    {
        private readonly Texture2D _testFailedTexture;
        private readonly Texture2D _testNoneTexture;
        private readonly Texture2D _testSuccessTexture;
        [NonSerialized] private int _currentId;

        public AssetRegulationViewerTreeView(TreeViewState treeViewState) : base(treeViewState)
        {
            _testSuccessTexture = EditorGUIUtility.Load("TestPassed") as Texture2D;
            _testFailedTexture = EditorGUIUtility.Load("TestFailed") as Texture2D;
            _testNoneTexture = EditorGUIUtility.Load("TestNormal") as Texture2D;
        }

        protected override IOrderedEnumerable<TreeViewItem> OrderItems(IList<TreeViewItem> items, int keyColumnIndex,
            bool ascending)
        {
            return items.OrderBy(x => x.displayName);
        }

        protected override string GetTextForSearch(TreeViewItem item, int columnIndex)
        {
            throw new NotSupportedException();
        }

        public AssetRegulationTestTreeViewItem AddAssetRegulationTestTreeViewItem(string assetPath, string testId,
            AssetRegulationTestStatus status, Texture2D icon)
        {
            var assetPathTreeViewItem = new AssetRegulationTestTreeViewItem(testId, status)
            {
                id = ++_currentId,
                displayName = assetPath
            };

            AddItemAndSetParent(assetPathTreeViewItem, -1);
            assetPathTreeViewItem.icon = icon;

            return assetPathTreeViewItem;
        }

        public AssetRegulationTestEntryTreeViewItem AddAssetRegulationTestEntryTreeViewItem(string id,
            string description,
            AssetRegulationTestStatus status,
            int parentId)
        {
            var treeViewItemId = ++_currentId;
            var assetRegulationTreeViewItem = new AssetRegulationTestEntryTreeViewItem(id, description, status)
            {
                id = treeViewItemId,
                displayName = description
            };

            AddItemAndSetParent(assetRegulationTreeViewItem, parentId);

            return assetRegulationTreeViewItem;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var status = GetStatus(args.item);

            var texture = GetTestResultTexture(status);

            var toggleRect = args.rowRect;
            toggleRect.x += GetContentIndent(args.item);
            toggleRect.width = 16f;
            GUI.DrawTexture(toggleRect, texture);

            extraSpaceBeforeIconAndLabel = toggleRect.width + 2f;

            base.RowGUI(args);
        }

        private AssetRegulationTestStatus GetStatus(TreeViewItem treeViewItem)
        {
            switch (treeViewItem)
            {
                case AssetRegulationTestTreeViewItem assetPathTreeViewItem:
                    return assetPathTreeViewItem.Status;
                case AssetRegulationTestEntryTreeViewItem regulationTreeViewItem:
                    return regulationTreeViewItem.Status;
                default:
                    return AssetRegulationTestStatus.None;
            }
        }

        private Texture2D GetTestResultTexture(AssetRegulationTestStatus status)
        {
            switch (status)
            {
                case AssetRegulationTestStatus.Success:
                    return _testSuccessTexture;
                case AssetRegulationTestStatus.Failed:
                    return _testFailedTexture;
                default:
                    return _testNoneTexture;
            }
        }
    }
}

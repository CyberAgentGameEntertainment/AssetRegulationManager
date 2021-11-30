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

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class AssetRegulationTreeView : TreeViewBase
    {
        private readonly Texture2D _testSuccessTexture;
        private readonly Texture2D _testFailedTexture;
        private readonly Texture2D _testNoneTexture;
        private int _currentId;

        internal AssetRegulationTreeView(TreeViewState treeViewState) : base(treeViewState)
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

        internal AssetPathTreeViewItem AddAssetPathTreeViewItem(string assetPath)
        {
            var assetPathTreeViewItem = new AssetPathTreeViewItem
            {
                id = ++_currentId,
                displayName = assetPath
            };

            AddItemAndSetParent(assetPathTreeViewItem, -1);
            
            return assetPathTreeViewItem;
        }

        internal AssetRegulationTreeViewItem AddAssetRegulationTreeViewItem(string id, string description,
            AssetRegulationTestResultType status,
            int parentId)
        {
            var treeViewItemId = ++_currentId;
            var assetRegulationTreeViewItem = new AssetRegulationTreeViewItem(id,description, status)
            {
                id = treeViewItemId,
                displayName = description
            };

            AddItemAndSetParent(assetRegulationTreeViewItem, parentId);

            return assetRegulationTreeViewItem;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var resultType = AssetRegulationTestResultType.None;
            if (args.item is AssetPathTreeViewItem assetPathTreeViewItem)
            {
                var assetRegulationTreeViewItems = assetPathTreeViewItem.children.OfType<AssetRegulationTreeViewItem>();

                if (assetRegulationTreeViewItems.All(x => x.Status == AssetRegulationTestResultType.Success))
                    resultType = AssetRegulationTestResultType.Success;
                if (assetRegulationTreeViewItems.Any(x => x.Status == AssetRegulationTestResultType.Failed))
                    resultType = AssetRegulationTestResultType.Failed;
            }

            if (args.item is AssetRegulationTreeViewItem regulationTreeViewItem)
                resultType = regulationTreeViewItem.Status;

            var texture = resultType switch
            {
                AssetRegulationTestResultType.Success => _testSuccessTexture,
                AssetRegulationTestResultType.Failed => _testFailedTexture,
                _ => _testNoneTexture
            };

            var toggleRect = args.rowRect;
            toggleRect.x += GetContentIndent(args.item);
            toggleRect.width = 16f;
            GUI.DrawTexture(toggleRect, texture);

            extraSpaceBeforeIconAndLabel = toggleRect.width + 2f;

            base.RowGUI(args);
        }

        internal IEnumerable<string> SelectionAssetRegulationTestIndex() => GetSelection().Select(GetItem)
            .Select(SearchAssetRegulationTestIndex).SelectMany(x => x).Distinct();

        private IEnumerable<string> SearchAssetRegulationTestIndex(TreeViewItem treeViewItem)
        {
            var entryIds = new List<string>();

            switch (treeViewItem)
            {
                case AssetPathTreeViewItem _:
                    entryIds.AddRange(treeViewItem.children.OfType<AssetRegulationTreeViewItem>().Select(assetRegulationTreeViewItem => assetRegulationTreeViewItem.EntryId));
                    break;
                case AssetRegulationTreeViewItem assetRegulationTreeViewItem:
                    entryIds.Add(assetRegulationTreeViewItem.EntryId);
                    break;
            }

            return entryIds;
        }
    }
}
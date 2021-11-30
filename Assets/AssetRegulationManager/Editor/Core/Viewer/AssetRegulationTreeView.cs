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
    internal sealed class RegulationTreeView : TreeViewBase
    {
        private readonly AssetRegulationTestIndexComparer _compare = new AssetRegulationTestIndexComparer();
        private readonly Texture2D _testFailedTexture;

        private readonly Dictionary<int, AssetRegulationTestIndex> _testIndexDictionary =
            new Dictionary<int, AssetRegulationTestIndex>();

        private readonly Texture2D _testNormalTexture;
        private readonly Texture2D _testPassedTexture;
        private int _assetPathTreeViewItemCount;
        private int _currentId;

        internal RegulationTreeView(TreeViewState treeViewState) : base(treeViewState)
        {
            _testPassedTexture = EditorGUIUtility.Load("TestPassed") as Texture2D;
            _testFailedTexture = EditorGUIUtility.Load("TestFailed") as Texture2D;
            _testNormalTexture = EditorGUIUtility.Load("TestNormal") as Texture2D;
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

        public new void ClearItems(bool invokeCallback = true)
        {
            base.ClearItems(invokeCallback);

            _assetPathTreeViewItemCount = 0;
        }

        internal AssetPathTreeViewItem AddAssetPathTreeViewItem(string path)
        {
            var assetPathTreeViewItem = new AssetPathTreeViewItem
            {
                id = ++_currentId,
                displayName = path
            };

            AddItemAndSetParent(assetPathTreeViewItem, -1);

            _assetPathTreeViewItemCount++;

            return assetPathTreeViewItem;
        }

        internal AssetRegulationTreeViewItem AddAssetRegulationTreeViewItem(string description,
            AssetRegulationTestResultType status,
            int parentId)
        {
            var treeViewItemId = ++_currentId;
            var assetRegulationTreeViewItem = new AssetRegulationTreeViewItem(description, status)
            {
                id = treeViewItemId,
                displayName = description
            };

            AddItemAndSetParent(assetRegulationTreeViewItem, parentId);

            _testIndexDictionary.Add(treeViewItemId,
                new AssetRegulationTestIndex(_assetPathTreeViewItemCount - 1, treeViewItemId - parentId - 1));

            return assetRegulationTreeViewItem;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var resultType = AssetRegulationTestResultType.None;
            if (args.item is AssetPathTreeViewItem assetPathTreeViewItem)
            {
                var regulationTreeViewItems = assetPathTreeViewItem.children.OfType<AssetRegulationTreeViewItem>();

                if (regulationTreeViewItems.All(x => x.Status == AssetRegulationTestResultType.Success))
                    resultType = AssetRegulationTestResultType.Success;
                if (regulationTreeViewItems.Any(x => x.Status == AssetRegulationTestResultType.Failed))
                    resultType = AssetRegulationTestResultType.Failed;
            }

            if (args.item is AssetRegulationTreeViewItem regulationTreeViewItem)
                resultType = regulationTreeViewItem.Status;

            var texture = resultType switch
            {
                AssetRegulationTestResultType.Success => _testPassedTexture,
                AssetRegulationTestResultType.Failed => _testFailedTexture,
                _ => _testNormalTexture
            };

            var toggleRect = args.rowRect;
            toggleRect.x += GetContentIndent(args.item);
            toggleRect.width = 16f;
            GUI.DrawTexture(toggleRect, texture);

            extraSpaceBeforeIconAndLabel = toggleRect.width + 2f;

            base.RowGUI(args);
        }

        internal IEnumerable<AssetRegulationTestIndex> SelectionAssetRegulationTestIndex()
        {
            return GetSelection().Select(GetItem).Select(SearchAssetRegulationTestIndex).SelectMany(x => x)
                .Distinct(_compare);
        }

        internal IEnumerable<AssetRegulationTestIndex> SearchAssetRegulationTestIndex(TreeViewItem treeViewItem)
        {
            var assetRegulationTestIndexes = new List<AssetRegulationTestIndex>();

            if (treeViewItem is AssetPathTreeViewItem)
                foreach (var child in treeViewItem.children)
                    assetRegulationTestIndexes.Add(_testIndexDictionary[child.id]);
            if (treeViewItem is AssetRegulationTreeViewItem)
                assetRegulationTestIndexes.Add(_testIndexDictionary[treeViewItem.id]);

            return assetRegulationTestIndexes;
        }
    }
}
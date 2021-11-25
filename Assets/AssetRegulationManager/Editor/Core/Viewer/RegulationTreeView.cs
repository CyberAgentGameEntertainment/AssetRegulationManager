// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssetRegulationManager.Editor.Foundation.EasyTreeView;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class RegulationTreeView : TreeViewBase
    {
        internal RegulationTreeView(TreeViewState treeViewState) : base(treeViewState)
        {
        }

        private int _currentId = 0;

        protected override IOrderedEnumerable<TreeViewItem> OrderItems(IList<TreeViewItem> items, int keyColumnIndex,
            bool ascending)
        {
            return items.OrderBy(x => x.displayName);
        }

        protected override string GetTextForSearch(TreeViewItem item, int columnIndex)
        {
            throw new NotSupportedException();
        }

        internal AssetPathTreeViewItem AddAssetPathTreeViewItem(string path)
        {
            var assetPathTreeViewItem = new AssetPathTreeViewItem()
            {
                id = ++_currentId,
                displayName = path
            };
            
            AddItemAndSetParent(assetPathTreeViewItem, -1);

            return assetPathTreeViewItem;
        }        
        
        internal AssetRegulationTreeViewItem AddAssetRegulationTreeViewItem(RegulationEntryDatum entryDatum, int parentId)
        {
            var assetRegulationTreeViewItem = new AssetRegulationTreeViewItem(entryDatum.MetaDatum, entryDatum.Explanation, entryDatum.ResultType.Value)
            {
                id = ++_currentId,
                displayName = entryDatum.Explanation
            };
            
            AddItemAndSetParent(assetRegulationTreeViewItem, parentId);

            return assetRegulationTreeViewItem;
        }
        
        protected override void RowGUI (RowGUIArgs args)
        {
            var resultType = TestResultType.None;
            if (args.item is AssetPathTreeViewItem assetPathTreeViewItem)
            {
                var regulationTreeViewItems = assetPathTreeViewItem.children.OfType<AssetRegulationTreeViewItem>();

                if (regulationTreeViewItems.All(x => x.ResultType == TestResultType.Success))
                    resultType = TestResultType.Success;
                if (regulationTreeViewItems.Any(x => x.ResultType == TestResultType.Failed))
                    resultType = TestResultType.Failed;
            }
            if (args.item is AssetRegulationTreeViewItem regulationTreeViewItem)
            {
                resultType = regulationTreeViewItem.ResultType;
            }

            var texture = resultType switch
            {
                TestResultType.Success => EditorGUIUtility.Load("TestPassed") as Texture2D,
                TestResultType.Failed => EditorGUIUtility.Load("TestFailed") as Texture2D,
                _ => EditorGUIUtility.Load("TestNormal") as Texture2D,
            };
            
            Rect toggleRect = args.rowRect;
            toggleRect.x += GetContentIndent(args.item);
            toggleRect.width = 16f;
            GUI.DrawTexture(toggleRect, texture);
                
            extraSpaceBeforeIconAndLabel = toggleRect.width + 2f;
            
            base.RowGUI(args);
        }

        internal IEnumerable<AssetRegulationTreeViewItem> SelectionAssetRegulationTreeViewItem()
        {
            return GetSelection().Select(GetItem).Select(SearchAssetRegulationTreeViewItem).SelectMany(x => x);
        }

        internal IEnumerable<AssetRegulationTreeViewItem> AllAssetRegulationTreeViewItem()
        {
            return rootItem.children.Select(SearchAssetRegulationTreeViewItem).SelectMany(x => x);
        }

        private IEnumerable<AssetRegulationTreeViewItem> SearchAssetRegulationTreeViewItem(TreeViewItem treeViewItem)
        {
            var assetRegulationTreeViewItems = new List<AssetRegulationTreeViewItem>();

            if (treeViewItem is AssetPathTreeViewItem)
            {
                return treeViewItem.children.OfType<AssetRegulationTreeViewItem>();
            }
            if (treeViewItem is AssetRegulationTreeViewItem assetRegulationTreeViewItem)
            {
                assetRegulationTreeViewItems.Add(assetRegulationTreeViewItem);
            }

            return assetRegulationTreeViewItems;
        }
    }
}
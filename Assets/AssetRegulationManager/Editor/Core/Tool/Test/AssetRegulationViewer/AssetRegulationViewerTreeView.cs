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

namespace AssetRegulationManager.Editor.Core.Tool.Test.AssetRegulationViewer
{
    internal sealed class AssetRegulationViewerTreeView : TreeViewBase
    {
        private readonly Texture2D _testFailedTexture;
        private readonly Texture2D _testWarningTexture;
        private readonly Texture2D _testNoneTexture;
        private readonly Texture2D _testSuccessTexture;
        [NonSerialized] private int _currentId;

        public AssetRegulationViewerTreeView(AssetRegulationViewerTreeViewState state) : base(state)
        {
            showAlternatingRowBackgrounds = true;
            _testFailedTexture = EditorGUIUtility.Load("TestFailed") as Texture2D;
            _testWarningTexture = EditorGUIUtility.Load("Warning") as Texture2D;
            _testNoneTexture = EditorGUIUtility.Load("TestNormal") as Texture2D;
            _testSuccessTexture = EditorGUIUtility.Load("TestPassed") as Texture2D;
            ColumnStates = state.ColumnStates;
            Reload();
        }

        public AssetRegulationTestTreeViewItem AddAssetRegulationTestTreeViewItem(string assetPath, string testId,
            AssetRegulationTestStatus status)
        {
            var assetPathTreeViewItem = new AssetRegulationTestTreeViewItem(testId, status, assetPath)
            {
                id = ++_currentId
            };

            AddItemAndSetParent(assetPathTreeViewItem, -1);

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

        protected override void CellGUI(int columnIndex, Rect cellRect, RowGUIArgs args)
        {
            switch ((Columns)columnIndex)
            {
                case Columns.Test:
                    TestCellGUI(cellRect, args);
                    break;
                case Columns.ActualValue:
                    GUI.Label(cellRect, GetText(args.item, columnIndex));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override IOrderedEnumerable<TreeViewItem> OrderItems(IList<TreeViewItem> items, int keyColumnIndex,
            bool ascending)
        {
            throw new NotSupportedException();
        }

        protected override string GetTextForSearch(TreeViewItem item, int columnIndex)
        {
            throw new NotSupportedException();
        }

        private void TestCellGUI(Rect rect, RowGUIArgs args)
        {
            var status = GetStatus(args.item);
            var texture = GetTestResultTexture(status);

            rect.xMin += GetContentIndent(args.item);
            var labelRect = rect;
            var statusIconRect = rect;
            statusIconRect.width = statusIconRect.height;
            labelRect.xMin += statusIconRect.width + 2.0f;
            GUI.DrawTexture(statusIconRect, texture);
            if (args.item is AssetRegulationTestTreeViewItem testItem)
            {
                var assetIconRect = statusIconRect;
                assetIconRect.x += assetIconRect.height + 2.0f;
                labelRect.xMin += assetIconRect.width + 2.0f;
                GUI.DrawTexture(assetIconRect, testItem.GetIcon());
                GUI.Label(labelRect, ((AssetRegulationTestTreeViewItem)args.item).DisplayName);
            }
            else
            {
                GUI.Label(labelRect, args.item.displayName);
            }
        }

        private static string GetText(TreeViewItem treeViewItem, int columnIndex)
        {
            switch ((Columns)columnIndex)
            {
                case Columns.Test:
                    return ((AssetRegulationTestTreeViewItem)treeViewItem).DisplayName;
                case Columns.ActualValue:
                    return GetActualValue(treeViewItem);
                default:
                    throw new ArgumentOutOfRangeException(nameof(columnIndex), columnIndex, null);
            }
        }

        private static AssetRegulationTestStatus GetStatus(TreeViewItem treeViewItem)
        {
            switch (treeViewItem)
            {
                case AssetRegulationTestTreeViewItem assetPathTreeViewItem:
                    return assetPathTreeViewItem.Status;
                case AssetRegulationTestEntryTreeViewItem regulationTreeViewItem:
                    return regulationTreeViewItem.Status;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static string GetActualValue(TreeViewItem treeViewItem)
        {
            switch (treeViewItem)
            {
                case AssetRegulationTestTreeViewItem _:
                    return string.Empty;
                case AssetRegulationTestEntryTreeViewItem regulationTreeViewItem:
                    return regulationTreeViewItem.ActualValue;
                default:
                    throw new InvalidOperationException();
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
                case AssetRegulationTestStatus.Warning:
                    return _testWarningTexture;
                case AssetRegulationTestStatus.None:
                    return _testNoneTexture;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        private enum Columns
        {
            Test,
            ActualValue
        }
    }
}

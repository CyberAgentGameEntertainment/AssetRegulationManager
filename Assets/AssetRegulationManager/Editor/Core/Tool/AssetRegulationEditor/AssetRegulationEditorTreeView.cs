// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.EasyTreeView;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorTreeView : TreeViewBase
    {
        [NonSerialized] private int _currentId;

        public AssetRegulationEditorTreeView(AssetRegulationEditorTreeViewState state) : base(state)
        {
            showAlternatingRowBackgrounds = true;
            ColumnStates = state.ColumnStates;
            Reload();
        }

        public AssetRegulationEditorTreeViewItem AddItem(AssetRegulation regulation)
        {
            var item = new AssetRegulationEditorTreeViewItem(regulation)
            {
                id = _currentId++,
            };
            AddItemAndSetParent(item, -1);
            return item;
        }

        public int GetRowsIndex(int id)
        {
            var rows = GetRows();

            for (var i = 0; i < rows.Count; i++)
                if (rows[i].id == id)
                    return i;

            return -1;
        }

        protected override void CellGUI(int columnIndex, Rect cellRect, RowGUIArgs args)
        {
            var item = (AssetRegulationEditorTreeViewItem) args.item;
            switch ((Columns) columnIndex)
            {
                case Columns.DisplayName:
                    base.CellGUI(columnIndex, cellRect, args);
                    break;
                case Columns.Filters:
                    GUI.Label(cellRect, GetText(item, columnIndex));
                    break;
                case Columns.Limitations:
                    GUI.Label(cellRect, GetText(item, columnIndex));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override IOrderedEnumerable<TreeViewItem> OrderItems(IList<TreeViewItem> items, int keyColumnIndex,
            bool ascending)
        {
            string KeySelector(TreeViewItem x)
            {
                return GetText((AssetRegulationEditorTreeViewItem) x, keyColumnIndex);
            }

            return ascending
                ? items.OrderBy(KeySelector, Comparer<string>.Create(EditorUtility.NaturalCompare))
                : items.OrderByDescending(KeySelector, Comparer<string>.Create(EditorUtility.NaturalCompare));
        }

        protected override string GetTextForSearch(TreeViewItem item, int columnIndex)
        {
            return GetText((AssetRegulationEditorTreeViewItem) item, columnIndex);
        }

        protected override bool CanRename(TreeViewItem item)
        {
            return true;
        }

        protected override void RenameEnded(RenameEndedArgs args)
        {
            if (args.acceptedRename)
            {
                var item = (AssetRegulationEditorTreeViewItem) GetItem(args.itemID);
                item.SetName(args.newName, true);
                Reload();
            }
        }

        private static string GetText(AssetRegulationEditorTreeViewItem item, int columnIndex)
        {
            switch ((Columns) columnIndex)
            {
                case Columns.DisplayName:
                    return item.displayName;
                case Columns.Filters:
                    return item.Regulation.AssetGroup.GetDescription();
                case Columns.Limitations:
                    return item.Regulation.AssetSpec.GetDescription();
                default:
                    throw new NotImplementedException();
            }
        }

        private enum Columns
        {
            DisplayName,
            Filters,
            Limitations
        }
    }
}

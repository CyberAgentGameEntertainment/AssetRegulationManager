﻿// --------------------------------------------------------------
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
        private const string DescriptionFieldName = "_description";
        private const string DefaultName = "New Asset Regulation";

        [NonSerialized] private int _currentId;

        public AssetRegulationEditorTreeView(AssetRegulationEditorTreeViewState state) : base(state)
        {
            showAlternatingRowBackgrounds = true;
            ColumnStates = state.ColumnStates;
            Reload();
        }

        public AssetRegulationEditorTreeViewItem AddItem(AssetRegulation regulation, SerializedProperty property)
        {
            var item = new AssetRegulationEditorTreeViewItem(regulation, property)
            {
                id = _currentId++,
                displayName = GetRegulationName(property)
            };
            AddItemAndSetParent(item, -1);
            return item;
        }

        private string GetRegulationName(SerializedProperty property)
        {
            var description = property.FindPropertyRelative(DescriptionFieldName).stringValue;
            return string.IsNullOrEmpty(description) ? DefaultName : description;
        }

        protected override void CellGUI(int columnIndex, Rect cellRect, RowGUIArgs args)
        {
            var item = (AssetRegulationEditorTreeViewItem)args.item;
            switch ((Columns)columnIndex)
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
                return GetText((AssetRegulationEditorTreeViewItem)x, keyColumnIndex);
            }

            return ascending
                ? items.OrderBy(KeySelector, Comparer<string>.Create(EditorUtility.NaturalCompare))
                : items.OrderByDescending(KeySelector, Comparer<string>.Create(EditorUtility.NaturalCompare));
        }

        protected override string GetTextForSearch(TreeViewItem item, int columnIndex)
        {
            return GetText((AssetRegulationEditorTreeViewItem)item, columnIndex);
        }

        protected override bool CanRename(TreeViewItem item)
        {
            return true;
        }

        protected override void RenameEnded(RenameEndedArgs args)
        {
            if (args.acceptedRename)
            {
                var item = (AssetRegulationEditorTreeViewItem)GetItem(args.itemID);
                item.Property.FindPropertyRelative(DescriptionFieldName).stringValue = args.newName;
                item.displayName = GetRegulationName(item.Property);
                Reload();
            }
        }

        private static string GetText(AssetRegulationEditorTreeViewItem item, int columnIndex)
        {
            switch ((Columns)columnIndex)
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
// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Foundation.EasyTreeView;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorTreeView : TreeViewBase
    {
        private const string DragType = "AssetRegulationEditorTreeView";

        [NonSerialized] private int _currentId;
        private readonly Dictionary<string, int> _regulationIdToItemIdMap = new Dictionary<string, int>();

        private readonly Subject<(AssetRegulationEditorTreeViewItem item, int newIndex)> _itemIndexChangedSubject =
            new Subject<(AssetRegulationEditorTreeViewItem item, int newIndex)>();
        
        public AssetRegulationEditorTreeView(AssetRegulationEditorTreeViewState state) : base(state)
        {
            showAlternatingRowBackgrounds = true;
            ColumnStates = state.ColumnStates;
            Reload();
        }

        public IObservable<(AssetRegulationEditorTreeViewItem item, int newIndex)> ItemIndexChangedAsObservable =>
            _itemIndexChangedSubject;

        public AssetRegulationEditorTreeViewItem GetItemByRegulationId(string regulationId)
        {
            var itemId = _regulationIdToItemIdMap[regulationId];
            return (AssetRegulationEditorTreeViewItem)GetItem(itemId);
        }

        public AssetRegulationEditorTreeViewItem AddItem(string regulationId)
        {
            var item = new AssetRegulationEditorTreeViewItem(regulationId)
            {
                id = _currentId++
            };
            _regulationIdToItemIdMap.Add(regulationId, item.id);
            AddItemAndSetParent(item, -1);
            return item;
        }

        public void RemoveItem(string regulationId, bool invokeCallback = true)
        {
            var id = _regulationIdToItemIdMap[regulationId];
            _regulationIdToItemIdMap.Remove(regulationId);
            RemoveItem(id, invokeCallback);
        }

        public void ClearItems()
        {
            _regulationIdToItemIdMap.Clear();
            base.ClearItems();
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
            var item = (AssetRegulationEditorTreeViewItem)args.item;
            switch ((Columns)columnIndex)
            {
                case Columns.DisplayName:
                    args.rowRect = cellRect;
                    base.CellGUI(columnIndex, cellRect, args);
                    break;
                case Columns.Targets:
                    GUI.Label(cellRect, GetText(item, columnIndex));
                    break;
                case Columns.Constraints:
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
                item.SetName(args.newName, true);
                Reload();
            }
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return true;
        }

        protected override bool CanBeParent(TreeViewItem item)
        {
            return false;
        }

        protected override bool CanStartDrag(CanStartDragArgs args)
        {
            return string.IsNullOrEmpty(searchString);
        }

        protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
        {
            var selections = args.draggedItemIDs;
            if (selections.Count <= 0) return;

            var items = GetRows()
                .Where(i => selections.Contains(i.id))
                .Select(x => (AssetRegulationEditorTreeViewItem)x)
                .ToArray();

            if (items.Length <= 0) return;

            DragAndDrop.PrepareStartDrag();
            DragAndDrop.SetGenericData(DragType, items);
            DragAndDrop.StartDrag(items.Length > 1 ? "<Multiple>" : items[0].displayName);
        }

        protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
        {
            if (args.performDrop)
            {
                var data = DragAndDrop.GetGenericData(DragType);
                var items = (AssetRegulationEditorTreeViewItem[])data;

                if (items == null || items.Length <= 0)
                    return DragAndDropVisualMode.None;

                switch (args.dragAndDropPosition)
                {
                    case DragAndDropPosition.BetweenItems:
                        var afterIndex = args.insertAtIndex;
                        foreach (var item in items)
                        {
                            var itemIndex = RootItem.children.IndexOf(item);
                            if (itemIndex < afterIndex) afterIndex--;

                            SetItemIndex(item.id, afterIndex, true);
                            afterIndex++;
                        }

                        SetSelection(items.Select(x => x.id).ToArray());

                        Reload();
                        break;
                    case DragAndDropPosition.UponItem:
                    case DragAndDropPosition.OutsideItems:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return DragAndDropVisualMode.Move;
        }

        public void SetItemIndex(int itemId, int index, bool notify)
        {
            var item = (AssetRegulationEditorTreeViewItem)GetItem(itemId);
            var children = RootItem.children;
            var itemIndex = RootItem.children.IndexOf(item);
            children.RemoveAt(itemIndex);
            children.Insert(index, item);
            if (notify) _itemIndexChangedSubject.OnNext((item, index));
        }

        private static string GetText(AssetRegulationEditorTreeViewItem item, int columnIndex)
        {
            switch ((Columns)columnIndex)
            {
                case Columns.DisplayName:
                    return item.displayName;
                case Columns.Targets:
                    return item.TargetsDescription;
                case Columns.Constraints:
                    return item.ConstraintsDescription;
                default:
                    throw new NotImplementedException();
            }
        }

        private enum Columns
        {
            DisplayName,
            Targets,
            Constraints
        }
    }
}

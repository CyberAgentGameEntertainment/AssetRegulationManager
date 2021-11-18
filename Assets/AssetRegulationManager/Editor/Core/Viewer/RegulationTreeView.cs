// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Foundation.EasyTreeView;
using UnityEditor.IMGUI.Controls;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class RegulationTreeView : TreeViewBase
    {
        internal RegulationTreeView(TreeViewState treeViewState) : base(treeViewState)
        {
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

        internal void AddTreeViewItem(IEnumerable<RegulationViewDatum> viewData)
        {
            var currentId = 0;

            foreach (var viewDatum in viewData)
            {
                var parentId = ++currentId;
                AddItemAndSetParent(new TreeViewItem {id = parentId, displayName = viewDatum.Path}, -1);
                foreach (var entryViewDatum in viewDatum.EntryViewData)
                    AddItemAndSetParent(new TreeViewItem {id = ++currentId, displayName = entryViewDatum.Explanation},
                        parentId);
            }
        }
    }
}
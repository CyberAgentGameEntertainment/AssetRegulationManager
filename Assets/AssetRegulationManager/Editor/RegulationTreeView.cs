using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace AssetsRegulation
{
    public class RegulationTreeView : TreeViewBase
    {
        public RegulationTreeView(TreeViewState treeViewState) : base(treeViewState)
        {
        }

        protected override IOrderedEnumerable<TreeViewItem> OrderItems(IList<TreeViewItem> items, int keyColumnIndex, bool @ascending)
        {
            return items.OrderBy(x => x.displayName);
        }

        protected override string GetTextForSearch(TreeViewItem item, int columnIndex)
        {
            throw new NotSupportedException();
        }
    }
}
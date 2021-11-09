using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace AssetsRegulation
{
    public class RegulationTreeView : TreeViewBase
    {
        private RegulationTreeElement[] _baseElements;

        public RegulationTreeView(TreeViewState treeViewState) : base(treeViewState)
        {
        }

        public void Setup(RegulationTreeElement[] baseElements)
        {
            _baseElements = baseElements;
            Reload();
        }

        protected override IOrderedEnumerable<TreeViewItem> OrderItems(IList<TreeViewItem> items, int keyColumnIndex, bool @ascending)
        {
            throw new NotImplementedException();
        }

        protected override string GetTextForSearch(TreeViewItem item, int columnIndex)
        {
            throw new NotImplementedException();
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            // 現在のRowsを取得
            var rows = GetRows() ?? new List<TreeViewItem>();
            rows.Clear();

            foreach (var baseElement in _baseElements)
            {
                var baseItem = CreateTreeViewItem(baseElement);
                // Itemはrootとrowsの両方に追加していく
                root.AddChild(baseItem);
                rows.Add(baseItem);
                if (baseElement.Children.Count >= 1)
                {
                    if (IsExpanded(baseItem.id))
                    {
                        AddChildrenRecursive(baseElement, baseItem, rows);
                    }
                    else
                    {
                        // 折りたたまれている場合はダミーのTreeViewItemを作成する（そういう決まり）
                        baseItem.children = CreateChildListForCollapsedParent();
                    }
                }
            }

            // depthを設定しなおす
            SetupDepthsFromParentsAndChildren(root);

            // rowsを返す
            return rows;
        }

        /// <summary>
        /// モデルとItemから再帰的に子Itemを作成・追加する
        /// </summary>
        private void AddChildrenRecursive(RegulationTreeElement element, TreeViewItem item, IList<TreeViewItem> rows)
        {
            foreach (var childElement in element.Children)
            {
                var childItem = CreateTreeViewItem(childElement);
                item.AddChild(childItem);
                rows.Add(childItem);
                if (childElement.Children.Count >= 1)
                {
                    if (IsExpanded(childElement.Id))
                    {
                        AddChildrenRecursive(childElement, childItem, rows);
                    }
                    else
                    {
                        // 折りたたまれている場合はダミーのTreeViewItemを作成する（そういう決まり）
                        childItem.children = CreateChildListForCollapsedParent();
                    }
                }
            }
        }

        /// <summary>
        /// ExampleTreeElementからTreeViewItemを作成する
        /// </summary>
        private TreeViewItem CreateTreeViewItem(RegulationTreeElement model)
        {
            return new TreeViewItem {id = model.Id, displayName = model.Name};
        }
    }
}
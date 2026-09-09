using System;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEditor.IMGUI.Controls;
#if UNITY_6000_5_OR_NEWER
using TreeViewItemT = UnityEditor.IMGUI.Controls.TreeViewItem<int>;
#else
using TreeViewItemT = UnityEditor.IMGUI.Controls.TreeViewItem;
#endif

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal static class AssetRegulationEditorTreeViewExtensions
    {
        public static IObservable<AssetRegulationEditorTreeViewItem> OnItemAddedAsObservable(this AssetRegulationEditorTreeView self)
        {
            return new AnonymousObservable<AssetRegulationEditorTreeViewItem>(observer =>
            {
                void OnNext(TreeViewItemT item)
                {
                    try
                    {
                        observer.OnNext((AssetRegulationEditorTreeViewItem)item);
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }
                }

                self.OnItemAdded += OnNext;
                return new Disposable(() => self.OnItemAdded -= OnNext);
            });
        }

        public static IObservable<AssetRegulationEditorTreeViewItem> OnItemRemovedAsObservable(this AssetRegulationEditorTreeView self)
        {
            return new AnonymousObservable<AssetRegulationEditorTreeViewItem>(observer =>
            {
                void OnNext(TreeViewItemT item)
                {
                    try
                    {
                        observer.OnNext((AssetRegulationEditorTreeViewItem)item);
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }
                }

                self.OnItemRemoved += OnNext;
                return new Disposable(() => self.OnItemRemoved -= OnNext);
            });
        }
    }
}

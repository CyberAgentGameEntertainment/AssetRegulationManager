using System;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEditor.IMGUI.Controls;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal static class AssetRegulationEditorTreeViewExtensions
    {
        public static IObservable<AssetRegulationEditorTreeViewItem> OnItemAddedAsObservable(this AssetRegulationEditorTreeView self)
        {
            return new AnonymousObservable<AssetRegulationEditorTreeViewItem>(observer =>
            {
                void OnNext(TreeViewItem item)
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
                void OnNext(TreeViewItem item)
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

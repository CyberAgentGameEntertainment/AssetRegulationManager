// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AssetRegulationManager.Editor.Foundation.ReorderableListUtility
{
    public interface ICustomReorderableList
    {
        bool DisplayAdd { get; set; }
        bool DisplayRemove { get; set; }
        float ElementHeight { get; set; }
        float HeaderHeight { get; set; }
        float FooterHeight { get; set; }
        bool ShowDefaultBackground { get; set; }
        SerializedProperty SerializedProperty { get; set; }
        IList List { get; set; }
        int Index { get; set; }
        bool Draggable { get; set; }
        int Count { get; }
        ReorderableList.DrawNoneElementCallback DrawNoneElementCallback { get; set; }
        ReorderableList.AddCallbackDelegate OnAddCallback { get; set; }
        ReorderableList.RemoveCallbackDelegate OnRemoveCallback { get; set; }

        event ReorderableList.HeaderCallbackDelegate DrawHeaderCallback;
        event ReorderableList.FooterCallbackDelegate DrawFooterCallback;
        event ReorderableList.ElementCallbackDelegate DrawElementCallback;
        event ReorderableList.ElementCallbackDelegate DrawElementBackgroundCallback;
        event ReorderableList.ElementHeightCallbackDelegate ElementHeightCallback;
        event ReorderableList.ReorderCallbackDelegateWithDetails OnReorderCallbackWithDetails;
        event ReorderableList.ReorderCallbackDelegate OnReorderCallback;
        event ReorderableList.SelectCallbackDelegate OnSelectCallback;
        event ReorderableList.AddDropdownCallbackDelegate OnAddDropdownCallback;
        event ReorderableList.DragCallbackDelegate OnMouseDragCallback;
        event ReorderableList.SelectCallbackDelegate OnMouseUpCallback;
        event ReorderableList.CanRemoveCallbackDelegate OnCanRemoveCallback;
        event ReorderableList.CanAddCallbackDelegate OnCanAddCallback;
        event ReorderableList.ChangedCallbackDelegate OnChangedCallback;

        void DoLayoutList();
        void DoList(Rect rect);
        float GetHeight();
    }
}

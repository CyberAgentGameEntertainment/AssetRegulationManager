// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AssetRegulationManager.Editor.Foundation.ReorderableListUtility
{
    /// <summary>
    ///     Collapsible <see cref="ReorderableList" />.
    /// </summary>
    public sealed class FoldoutReorderableList : ICustomReorderableList
    {
        private readonly ReorderableList _closedList;
        private readonly ReorderableList _openList;

        public FoldoutReorderableList(IList elements, Type elementType)
        {
            _openList = new ReorderableList(elements, elementType);
            _closedList = new ReorderableList(elements, elementType);
            SetupOpenList(_openList);
            SetupClosedList(_closedList);
        }

        public FoldoutReorderableList(SerializedObject serializedObject, SerializedProperty elements)
        {
            _openList = new ReorderableList(serializedObject, elements);
            _closedList = new ReorderableList(serializedObject, elements);
            SetupOpenList(_openList);
            SetupClosedList(_closedList);
        }

        public bool Foldout { get; private set; } = true;

        public string Title { get; set; }

        public event ReorderableList.HeaderCallbackDelegate DrawHeaderCallback
        {
            add
            {
                _openList.drawHeaderCallback += value;
                _closedList.drawHeaderCallback += value;
            }
            remove
            {
                _openList.drawHeaderCallback -= value;
                _closedList.drawHeaderCallback -= value;
            }
        }

        public event ReorderableList.FooterCallbackDelegate DrawFooterCallback
        {
            add
            {
                _openList.drawFooterCallback += value;
                _closedList.drawFooterCallback += value;
            }
            remove
            {
                _openList.drawFooterCallback -= value;
                _closedList.drawFooterCallback -= value;
            }
        }

        public event ReorderableList.ElementCallbackDelegate DrawElementCallback
        {
            add => _openList.drawElementCallback += value;
            remove => _openList.drawElementCallback -= value;
        }

        public event ReorderableList.ElementCallbackDelegate DrawElementBackgroundCallback
        {
            add => _openList.drawElementBackgroundCallback += value;
            remove => _openList.drawElementBackgroundCallback -= value;
        }

        public ReorderableList.DrawNoneElementCallback DrawNoneElementCallback
        {
            get => _openList.drawNoneElementCallback;
            set => _openList.drawNoneElementCallback = value;
        }

        public event ReorderableList.ElementHeightCallbackDelegate ElementHeightCallback
        {
            add => _openList.elementHeightCallback += value;
            remove => _openList.elementHeightCallback -= value;
        }

        public event ReorderableList.ReorderCallbackDelegateWithDetails OnReorderCallbackWithDetails
        {
            add => _openList.onReorderCallbackWithDetails += value;
            remove => _openList.onReorderCallbackWithDetails -= value;
        }

        public event ReorderableList.ReorderCallbackDelegate OnReorderCallback
        {
            add => _openList.onReorderCallback += value;
            remove => _openList.onReorderCallback -= value;
        }

        public event ReorderableList.SelectCallbackDelegate OnSelectCallback
        {
            add => _openList.onSelectCallback += value;
            remove => _openList.onSelectCallback -= value;
        }

        public ReorderableList.AddCallbackDelegate OnAddCallback
        {
            get => _openList.onAddCallback;
            set => _openList.onAddCallback = value;
        }

        public event ReorderableList.AddDropdownCallbackDelegate OnAddDropdownCallback
        {
            add => _openList.onAddDropdownCallback += value;
            remove => _openList.onAddDropdownCallback -= value;
        }

        public ReorderableList.RemoveCallbackDelegate OnRemoveCallback
        {
            get => _openList.onRemoveCallback;
            set => _openList.onRemoveCallback = value;
        }

        public event ReorderableList.DragCallbackDelegate OnMouseDragCallback
        {
            add
            {
                _openList.onMouseDragCallback += value;
                _closedList.onMouseDragCallback += value;
            }
            remove
            {
                _openList.onMouseDragCallback -= value;
                _closedList.onMouseDragCallback -= value;
            }
        }

        public event ReorderableList.SelectCallbackDelegate OnMouseUpCallback
        {
            add
            {
                _openList.onMouseUpCallback += value;
                _closedList.onMouseUpCallback += value;
            }
            remove
            {
                _openList.onMouseUpCallback -= value;
                _closedList.onMouseUpCallback -= value;
            }
        }

        public event ReorderableList.CanRemoveCallbackDelegate OnCanRemoveCallback
        {
            add => _openList.onCanRemoveCallback += value;
            remove => _openList.onCanRemoveCallback -= value;
        }

        public event ReorderableList.CanAddCallbackDelegate OnCanAddCallback
        {
            add => _openList.onCanAddCallback += value;
            remove => _openList.onCanAddCallback -= value;
        }

        public event ReorderableList.ChangedCallbackDelegate OnChangedCallback
        {
            add => _openList.onChangedCallback += value;
            remove => _openList.onChangedCallback -= value;
        }

        public bool DisplayAdd
        {
            get => _openList.displayAdd;
            set
            {
                _openList.displayAdd = value;
                _closedList.displayAdd = value;
            }
        }

        public bool DisplayRemove
        {
            get => _openList.displayRemove;
            set
            {
                _openList.displayRemove = value;
                _closedList.displayRemove = value;
            }
        }

        public float ElementHeight
        {
            get => _openList.elementHeight;
            set => _openList.elementHeight = value;
        }

        public float HeaderHeight
        {
            get => _openList.headerHeight;
            set
            {
                _openList.headerHeight = value;
                _closedList.headerHeight = value;
            }
        }

        public float FooterHeight
        {
            get => _openList.footerHeight;
            set
            {
                _openList.footerHeight = value;
                _closedList.footerHeight = value;
            }
        }

        public bool ShowDefaultBackground
        {
            get => _openList.showDefaultBackground;
            set
            {
                _openList.showDefaultBackground = value;
                _closedList.showDefaultBackground = value;
            }
        }

        public SerializedProperty SerializedProperty
        {
            get => _openList.serializedProperty;
            set
            {
                _openList.serializedProperty = value;
                _closedList.serializedProperty = value;
            }
        }

        public IList List
        {
            get => _openList.list;
            set => _openList.list = value;
        }

        public int Index
        {
            get => _openList.index;
            set => _openList.index = value;
        }

        public bool Draggable
        {
            get => _openList.draggable;
            set => _openList.draggable = value;
        }

        public int Count => _closedList.count;

        public void DoLayoutList()
        {
            if (Foldout)
            {
                _openList.DoLayoutList();
            }
            else
            {
                _closedList.DoLayoutList();
            }
        }

        public void DoList(Rect rect)
        {
            if (Foldout)
            {
                _openList.DoList(rect);
            }
            else
            {
                _closedList.DoList(rect);
            }
        }

        public float GetHeight()
        {
            if (Foldout)
            {
                return _openList.GetHeight();
            }

            return _closedList.GetHeight();
        }

        private void SetupOpenList(ReorderableList reorderableList)
        {
            reorderableList.drawHeaderCallback += rect =>
            {
                rect.xMin += 10;
                var title = string.IsNullOrEmpty(Title) ? reorderableList.serializedProperty.displayName : Title;
                Foldout = EditorGUI.Foldout(rect, Foldout, title, true);
            };
        }

        private void SetupClosedList(ReorderableList reorderableList)
        {
            reorderableList.drawHeaderCallback += rect =>
            {
                rect.xMin += 10;
                var title = string.IsNullOrEmpty(Title) ? reorderableList.serializedProperty.displayName : Title;
                Foldout = EditorGUI.Foldout(rect, Foldout, title, true);
            };
            reorderableList.drawElementCallback = (rect, index, active, focused) => { };
            reorderableList.elementHeight = 0;
            reorderableList.onCanAddCallback = _ => false;
            reorderableList.onCanRemoveCallback = _ => false;
            reorderableList.draggable = false;
        }
    }
}

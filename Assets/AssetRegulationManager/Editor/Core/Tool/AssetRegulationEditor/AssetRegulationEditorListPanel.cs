using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Shared;
using AssetRegulationManager.Editor.Foundation.EasyTreeView;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorListPanel : IDisposable
    {
        private readonly Subject<Empty> _addButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _assetSelectButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _rightClickCopyConstraintsDescriptionMenuClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _rightClickCopyTargetsDescriptionMenuClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _rightClickCreateMenuClickedSubject = new Subject<Empty>();

        private readonly Subject<IEnumerable<AssetRegulationEditorTreeViewItem>> _rightClickRemoveMenuClickedSubject =
            new Subject<IEnumerable<AssetRegulationEditorTreeViewItem>>();

        private readonly TreeViewSearchField _searchField;

        public AssetRegulationEditorListPanel(AssetRegulationEditorTreeViewState treeViewState)
        {
            TreeView = new AssetRegulationEditorTreeView(treeViewState);
            _searchField = new TreeViewSearchField(TreeView);
            AddContextMenuToTreeView(TreeView);
        }

        public string AssetName { get; set; }

        public IObservable<Empty> RightClickCreateMenuClickedAsObservable => _rightClickCreateMenuClickedSubject;

        public IObservable<Empty> RightClickCopyTargetsDescriptionMenuClickedSubject =>
            _rightClickCopyTargetsDescriptionMenuClickedSubject;

        public IObservable<Empty> RightClickCopyConstraintsDescriptionMenuClickedSubject =>
            _rightClickCopyConstraintsDescriptionMenuClickedSubject;

        public IObservable<IEnumerable<AssetRegulationEditorTreeViewItem>> RightClickRemoveMenuClickedAsObservable =>
            _rightClickRemoveMenuClickedSubject;

        public AssetRegulationEditorTreeView TreeView { get; }
        public IObservable<Empty> AddButtonClickedAsObservable => _addButtonClickedSubject;
        public IObservable<Empty> AssetSelectButtonClickedAsObservable => _assetSelectButtonClickedSubject;

        public void Dispose()
        {
            _addButtonClickedSubject.Dispose();
            _rightClickCreateMenuClickedSubject.Dispose();
            _rightClickRemoveMenuClickedSubject.Dispose();
            _rightClickCopyTargetsDescriptionMenuClickedSubject.Dispose();
            _rightClickCopyConstraintsDescriptionMenuClickedSubject.Dispose();
            _assetSelectButtonClickedSubject.Dispose();
            TreeView.Dispose();
        }

        public void DoLayout()
        {
            EditorGUILayout.BeginVertical();

            DrawToolbar();

            var treeViewRect =
                GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            TreeView.OnGUI(treeViewRect);

            EditorGUILayout.EndVertical();
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                // Add Button
                var addButtonRect = GUILayoutUtility.GetRect(24, 20);
                var addButtonImageRect = addButtonRect;
                addButtonImageRect.xMin += 2;
                addButtonImageRect.xMax -= 2;
                var plusIconTexture = EditorGUIUtility.IconContent(EditorGUIUtil.ToolbarPlusIconName).image;
                GUI.DrawTexture(addButtonImageRect, plusIconTexture, ScaleMode.StretchToFill);
                if (GUI.Button(addButtonRect, "", GUIStyle.none))
                    _addButtonClickedSubject.OnNext(Empty.Default);

                // Search Field
                _searchField.OnToolbarGUI();

                GUILayout.FlexibleSpace();

                // Dropdown
                if (EditorGUILayout.DropdownButton(new GUIContent(AssetName), FocusType.Passive,
                        GUILayout.MinWidth(150)))
                    _assetSelectButtonClickedSubject.OnNext(Empty.Default);
            }
        }

        private void AddContextMenuToTreeView(AssetRegulationEditorTreeView treeView)
        {
            treeView.RightClickMenuRequested = () =>
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Create"), false,
                    () => _rightClickCreateMenuClickedSubject.OnNext(Empty.Default));
                if (treeView.HasSelection())
                    menu.AddItem(new GUIContent("Remove"), false,
                        () =>
                        {
                            var items = treeView
                                .GetSelection()
                                .Where(x => treeView.HasItem(x))
                                .Select(x => (AssetRegulationEditorTreeViewItem)treeView.GetItem(x));
                            _rightClickRemoveMenuClickedSubject.OnNext(items);
                        });
                else
                    menu.AddDisabledItem(new GUIContent("Remove"));
                menu.AddItem(new GUIContent("Copy Targets Description"), false,
                    () => _rightClickCopyTargetsDescriptionMenuClickedSubject.OnNext(Empty.Default));
                menu.AddItem(new GUIContent("Copy Constraints Description"), false,
                    () => _rightClickCopyConstraintsDescriptionMenuClickedSubject.OnNext(Empty.Default));
                return menu;
            };
        }
    }
}

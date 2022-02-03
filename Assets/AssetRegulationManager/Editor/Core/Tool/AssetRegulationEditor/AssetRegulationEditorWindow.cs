// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.EasyTreeView;
using AssetRegulationManager.Editor.Foundation.EditorGUISplitView;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorWindow : EditorWindow
    {
        private const string WindowName = "Asset Regulation Editor";
        private const string RegulationsFieldName = "_regulations";

        [SerializeField] private AssetRegulationEditorTreeViewState _treeViewState;
        [SerializeField] private AssetRegulationSettings _settings;
        [SerializeField] private EditorGUISplitView _splitView;

        private AssetRegulationEditorInspectorDrawer _inspectorDrawer;
        private TreeViewSearchField _searchField;
        private SerializedObject _settingsSo;
        private AssetRegulationEditorTreeView _treeView;

        private void OnEnable()
        {
            if (_treeViewState == null) _treeViewState = new AssetRegulationEditorTreeViewState();

            _treeView = new AssetRegulationEditorTreeView(_treeViewState);
            _treeView.OnSelectionChanged += OnSelectionChanged;
            _searchField = new TreeViewSearchField(_treeView);

            if (_splitView == null) _splitView = new EditorGUISplitView(LayoutDirection.Horizontal, 600, 150, 250);

            minSize = new Vector2(500, 200);

            if (_settings != null) Setup(_settings);

            if (_treeView.GetSelection().Count >= 1) OnSelectionChanged(_treeView.GetSelection());
        }

        private void OnDisable()
        {
            _treeView.OnSelectionChanged -= OnSelectionChanged;
        }

        private void OnGUI()
        {
            // Nothing will be drawn if a ScriptableObject is deleted.
            if (_settingsSo == null || _settingsSo.targetObject == null) return;

            _settingsSo.Update();

            DrawToolbar();

            var contentsRect = new Rect(0, 0, position.width, position.height);
            contentsRect.yMin = GUILayoutUtility.GetLastRect().height;
            if (_splitView.DrawGUI(contentsRect, DrawTreeView, DrawInspector)) Repaint();

            _settingsSo.ApplyModifiedProperties();
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                if (GUILayout.Button("Add", EditorStyles.toolbarButton, GUILayout.MaxWidth(100))) AddNewRegulation();

                GUI.enabled = _treeView.HasSelection();
                if (GUILayout.Button("Remove", EditorStyles.toolbarButton, GUILayout.MaxWidth(100)))
                    RemoveSelectedRegulations();

                GUI.enabled = true;

                _searchField.OnToolbarGUI();

                GUILayout.FlexibleSpace();
            }
        }

        private void AddNewRegulation()
        {
            var regulation = new AssetRegulation();
            _settings.Regulations.Add(regulation);
            _settingsSo.Update();
            _treeView.AddItem(regulation);
        }

        private void RemoveSelectedRegulations()
        {
            foreach (var itemId in _treeView.GetSelection())
            {
                var item = (AssetRegulationEditorTreeViewItem) _treeView.GetItem(itemId);
                _settings.Regulations.Remove(item.Regulation);
                _settingsSo.Update();
                _treeView.RemoveItem(itemId);
            }

            _treeView.SetSelection(new List<int>());
            UnSelect();
        }

        private void DrawTreeView(Rect rect)
        {
            _treeView.Reload();
            _treeView.OnGUI(rect);
        }

        private void DrawInspector(Rect rect)
        {
            _inspectorDrawer?.OnGUI(rect);
        }

        private void OnSelectionChanged(IList<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                UnSelect();
                return;
            }

            var id = ids.First();
            var index = _treeView.GetRowsIndex(id);

            OnSelectionChanged(index);
        }

        private void OnSelectionChanged(int index)
        {
            if (index == -1)
            {
                UnSelect();
                return;
            }

            var regulationsProperty = _settingsSo.FindProperty(RegulationsFieldName);
            var assetGroupProperty = regulationsProperty.GetArrayElementAtIndex(index);

            _inspectorDrawer = new AssetRegulationEditorInspectorDrawer(assetGroupProperty);
        }

        private void UnSelect()
        {
            _inspectorDrawer = null;
        }

        private void Setup(AssetRegulationSettings settings)
        {
            var so = new SerializedObject(settings);

            _treeView.ClearItems();
            foreach (var regulation in settings.Regulations) _treeView.AddItem(regulation);

            _treeView.SetSelection(new List<int>());

            _settings = settings;
            _settingsSo = so;
        }

        public static void Open(AssetRegulationSettings settings)
        {
            var window = GetWindow<AssetRegulationEditorWindow>(WindowName);
            window.Setup(settings);
        }
    }
}

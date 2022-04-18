// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Foundation.EditorSplitView.Editor;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    /// <summary>
    ///     <see cref="EditorWindow" /> to edit the asset regulations.
    /// </summary>
    internal sealed class AssetRegulationEditorWindow : EditorWindow
    {
        public enum InspectorTabType
        {
            Targets,
            Regulations
        }

        public enum ViewType
        {
            Editor,
            Empty
        }
        
        private const string WindowName = "Asset Regulation Editor";
        private const string TargetsTabName = "Targets";
        private const string ConstraintsTabName = "Constraints";

        [SerializeField] private EditorGUILayoutSplitView _splitView;
        [SerializeField] private AssetRegulationEditorTreeViewState _treeViewState;
        [SerializeField] private AssetRegulationSetStore _store;

        [SerializeField] private InspectorTabTypeObservableProperty _activeInspectorTabType =
            new InspectorTabTypeObservableProperty();

        private readonly Subject<Empty> _redoShortcutExecutedSubject = new Subject<Empty>();

        private readonly Subject<IEnumerable<AssetRegulationEditorTreeViewItem>> _removeShortcutExecutedSubject =
            new Subject<IEnumerable<AssetRegulationEditorTreeViewItem>>();

        private readonly Subject<Empty> _undoShortcutExecutedSubject = new Subject<Empty>();

        private AssetRegulationEditorApplication _application;

        public ViewType ActiveViewType { get; set; }
        public IReadOnlyObservableProperty<InspectorTabType> ActiveInspectorTabType => _activeInspectorTabType;
        public IObservable<Empty> UndoShortcutExecutedAsObservable => _undoShortcutExecutedSubject;
        public IObservable<Empty> RedoShortcutExecutedAsObservable => _redoShortcutExecutedSubject;

        public IObservable<IEnumerable<AssetRegulationEditorTreeViewItem>> RemoveShortcutExecutedAsObservable =>
            _removeShortcutExecutedSubject;

        public AssetRegulationEditorEmptyPanel EmptyPanel { get; private set; }
        public AssetRegulationEditorListPanel ListPanel { get; private set; }
        public AssetRegulationEditorTargetsPanel TargetsPanel { get; private set; }
        public AssetRegulationEditorConstraintsPanel ConstraintsPanel { get; private set; }

        private void OnEnable()
        {
            Setup(_store);
        }

        private void OnDisable()
        {
            Cleanup(false);
        }

        private void Cleanup(bool deleteStates)
        {
            _application?.Dispose();
            ListPanel.Dispose();
            TargetsPanel.Dispose();
            ConstraintsPanel.Dispose();
            if (deleteStates)
            {
                _splitView = null;
                _treeViewState = null;
            }
        }

        private void Setup(AssetRegulationSetStore store)
        {
            if (_splitView == null)
                _splitView = new EditorGUILayoutSplitView(LayoutDirection.Horizontal, 0.6f, 250, 350);

            if (_treeViewState == null)
                _treeViewState = new AssetRegulationEditorTreeViewState();

            minSize = new Vector2(600, 200);

            EmptyPanel = new AssetRegulationEditorEmptyPanel();
            ListPanel = new AssetRegulationEditorListPanel(_treeViewState);
            TargetsPanel = new AssetRegulationEditorTargetsPanel();
            ConstraintsPanel = new AssetRegulationEditorConstraintsPanel();

            if (store != null)
            {
                _store = store;
                _application?.Dispose();
                _application = new AssetRegulationEditorApplication(store, this);
            }
        }

        private void OnDestroy()
        {
            _activeInspectorTabType.Dispose();
        }

        private void OnGUI()
        {
            HandleShortcuts();

            switch (ActiveViewType)
            {
                case ViewType.Editor:
                    DrawDefaultPanel();
                    break;
                case ViewType.Empty:
                    EmptyPanel.DoLayout();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleShortcuts()
        {
            var e = Event.current;
            if (GetEventAction(e) && e.type == EventType.KeyDown && e.keyCode == KeyCode.Z)
            {
                _undoShortcutExecutedSubject.OnNext(Empty.Default);
                e.Use();
            }

            if (GetEventAction(e) && e.type == EventType.KeyDown && e.keyCode == KeyCode.Y)
            {
                _redoShortcutExecutedSubject.OnNext(Empty.Default);
                e.Use();
            }

            if (GetEventAction(e) && e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete)
            {
                var treeView = ListPanel.TreeView;
                var items = treeView
                    .GetSelection()
                    .Where(x => treeView.HasItem(x))
                    .Select(x => (AssetRegulationEditorTreeViewItem)treeView.GetItem(x));
                _removeShortcutExecutedSubject.OnNext(items);
                e.Use();
            }
        }

        private void DrawDefaultPanel()
        {
            _splitView.Begin();

            ListPanel.DoLayout();

            if (_splitView.Split())
                Repaint();

            DrawInspector();

            _splitView.End();
        }

        private void DrawInspector()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                using (var ccs = new EditorGUI.ChangeCheckScope())
                {
                    var isActive = _activeInspectorTabType.Value == InspectorTabType.Targets;
                    isActive = GUILayout.Toggle(isActive, TargetsTabName, EditorStyles.toolbarButton,
                        GUILayout.Width(80));
                    if (ccs.changed && isActive)
                        _activeInspectorTabType.Value = InspectorTabType.Targets;
                }

                using (var ccs = new EditorGUI.ChangeCheckScope())
                {
                    var isActive = _activeInspectorTabType.Value == InspectorTabType.Regulations;
                    isActive = GUILayout.Toggle(isActive, ConstraintsTabName, EditorStyles.toolbarButton,
                        GUILayout.Width(80));
                    if (ccs.changed && isActive)
                        _activeInspectorTabType.Value = InspectorTabType.Regulations;
                }

                GUILayout.FlexibleSpace();
            }

            var labelWidth = EditorGUIUtility.labelWidth;
            var windowWidth = EditorGUIUtility.currentViewWidth;
            var leftPanelWidth = windowWidth - windowWidth * _splitView.NormalizedPosition;
            EditorGUIUtility.labelWidth = leftPanelWidth / 3;
            switch (_activeInspectorTabType.Value)
            {
                case InspectorTabType.Targets:
                    DrawTargetsInspector();
                    break;
                case InspectorTabType.Regulations:
                    DrawRegulationsInspector();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            EditorGUIUtility.labelWidth = labelWidth;
        }

        private void DrawTargetsInspector()
        {
            TargetsPanel.DoLayout();
        }

        private void DrawRegulationsInspector()
        {
            ConstraintsPanel.DoLayout();
        }

        public static AssetRegulationEditorWindow Open(AssetRegulationSetStore store)
        {
            var window = GetWindow<AssetRegulationEditorWindow>(WindowName);
            window.Cleanup(true);
            window.Setup(store);
            return window;
        }

        private bool GetEventAction(Event e)
        {
#if UNITY_EDITOR_WIN
            return e.control;
#else
            return e.command;
#endif
        }

        [Serializable]
        private sealed class InspectorTabTypeObservableProperty : ObservableProperty<InspectorTabType>
        {
        }
    }
}

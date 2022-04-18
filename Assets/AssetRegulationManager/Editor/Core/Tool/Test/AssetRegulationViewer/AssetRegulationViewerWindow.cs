// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.IO;
using AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.Test.AssetRegulationViewer
{
    internal sealed class AssetRegulationViewerWindow : EditorWindow
    {
        private const int InputRefreshMillis = 500;
        private const string WindowName = "Asset Regulation Viewer";

        [SerializeField] private AssetRegulationViewerTreeViewState _treeViewState;
        [SerializeField] private string _searchText;

        private readonly Subject<string> _assetPathOrFilterChangedSubject = new Subject<string>();
        private readonly Subject<Empty> _checkAllButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _checkSelectedAddButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _exportAsJsonButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _exportAsTextButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<string> _refreshButtonClickedSubject = new Subject<string>();

        private AssetRegulationTestApplication _application;
        private bool _isSearchTextDirty;
        private DateTime _lastSearchFieldUpdateTime;
        private SearchField _searchField;
        private AssetRegulationViewerState _state;

        public IObservable<string> AssetPathOrFilterChangedAsObservable => _assetPathOrFilterChangedSubject;
        public IObservable<string> RefreshButtonClickedAsObservable => _refreshButtonClickedSubject;
        public IObservable<Empty> CheckAllButtonClickedAsObservable => _checkAllButtonClickedSubject;
        public IObservable<Empty> CheckSelectedAddButtonClickedAsObservable => _checkSelectedAddButtonClickedSubject;
        public IObservable<Empty> ExportAsTextButtonClickedAsObservable => _exportAsTextButtonClickedSubject;
        public IObservable<Empty> ExportAsJsonButtonClickedAsObservable => _exportAsJsonButtonClickedSubject;
        public BoolObservableProperty ExcludeEmptyTests { get; } = new BoolObservableProperty();
        public AssetRegulationViewerTreeView TreeView { get; private set; }
        public string SelectedAssetPath { get; set; }

        private void Update()
        {
            if (_isSearchTextDirty &&
                (DateTime.Now - _lastSearchFieldUpdateTime).TotalMilliseconds >= InputRefreshMillis)
            {
                OnAssetPathOrFilterChanged();
                _isSearchTextDirty = false;
                Repaint();
            }
        }

        private void OnEnable()
        {
            if (_treeViewState == null)
            {
                _treeViewState = new AssetRegulationViewerTreeViewState();
            }
            
            TreeView = new AssetRegulationViewerTreeView(_treeViewState);
            _searchField = new SearchField();
            _searchField.downOrUpArrowKeyPressed += TreeView.SetFocusAndEnsureSelectedItem;

            _application = AssetRegulationTestApplication.RequestInstance();
            _application.AssetRegulationViewerController.Setup(this);
            _application.AssetRegulationViewerPresenter.Setup(this);

            OnAssetPathOrFilterChanged();
            _isSearchTextDirty = false;
        }

        private void OnDisable()
        {
            _application.AssetRegulationViewerController.Cleanup();
            _application.AssetRegulationViewerPresenter.Cleanup();
            _searchField.downOrUpArrowKeyPressed -= TreeView.SetFocusAndEnsureSelectedItem;
            AssetRegulationTestApplication.ReleaseInstance();
        }

        private void OnGUI()
        {
            // Draw Toolbar
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.FlexibleSpace();
                using (var ccs = new EditorGUI.ChangeCheckScope())
                {
                    _searchText = _searchField.OnToolbarGUI(_searchText, GUILayout.MaxWidth(300));
                    if (ccs.changed)
                    {
                        _lastSearchFieldUpdateTime = DateTime.Now;
                        _isSearchTextDirty = true;
                    }
                }

                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.MaxWidth(100)))
                {
                    _refreshButtonClickedSubject.OnNext(_searchText);
                }

                ExcludeEmptyTests.Value = GUILayout.Toggle(ExcludeEmptyTests.Value, "Hide Empty",
                    EditorStyles.toolbarButton, GUILayout.MaxWidth(100));

                if (GUILayout.Button("Check All", EditorStyles.toolbarButton, GUILayout.MaxWidth(100)))
                {
                    _checkAllButtonClickedSubject.OnNext(Empty.Default);
                }

                if (GUILayout.Button("Check Selected", EditorStyles.toolbarButton, GUILayout.MaxWidth(100)))
                {
                    _checkSelectedAddButtonClickedSubject.OnNext(Empty.Default);
                }

                if (GUILayout.Button("Export Result", EditorStyles.toolbarButton, GUILayout.MaxWidth(100)))
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("As Text"), false,
                        () => _exportAsTextButtonClickedSubject.OnNext(Empty.Default));
                    menu.AddItem(new GUIContent("As Json"), false,
                        () => _exportAsJsonButtonClickedSubject.OnNext(Empty.Default));
                    menu.ShowAsContext();
                }
            }

            // Draw Tree View
            var treeViewRect = GUILayoutUtility.GetRect(0, float.MaxValue, 0, float.MaxValue);

            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                if (!string.IsNullOrEmpty(SelectedAssetPath))
                {
                    var icon = (Texture2D)AssetDatabase.GetCachedIcon(SelectedAssetPath);
                    GUILayout.Label(new GUIContent(SelectedAssetPath, icon),
                        GUILayout.Height(EditorGUIUtility.singleLineHeight));
                }
            }

            TreeView.Reload();
            TreeView.OnGUI(treeViewRect);
        }

        private void OnAssetPathOrFilterChanged()
        {
            _assetPathOrFilterChangedSubject.OnNext(_searchText);
        }

        [MenuItem("Window/" + WindowName)]
        private static void Open()
        {
            GetWindow<AssetRegulationViewerWindow>(WindowName);
        }

        public static void Open(string searchText)
        {
            var window = GetWindow<AssetRegulationViewerWindow>(WindowName);
            window._searchText = searchText;
            window._lastSearchFieldUpdateTime = DateTime.Now;
            window._isSearchTextDirty = true;
        }

        [MenuItem("Assets/" + WindowName)]
        private static void OpenInProjectView()
        {
            var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(assetPath))
            {
                return;
            }

            var assetName = Path.GetFileNameWithoutExtension(assetPath);
            Open(assetName);
        }
    }
}

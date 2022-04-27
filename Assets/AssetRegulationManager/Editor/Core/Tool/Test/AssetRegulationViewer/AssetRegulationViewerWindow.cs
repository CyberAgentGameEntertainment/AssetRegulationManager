// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.IO;
using AssetRegulationManager.Editor.Core.Shared;
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
        private const string PlaceHolder = "ExampleTexture  t:Texture  l:ExampleLabel";

        [SerializeField] private AssetRegulationViewerTreeViewState _treeViewState;
        [SerializeField] private string _searchText;

        private readonly Subject<string> _assetPathOrFilterChangedSubject = new Subject<string>();
        private readonly Subject<Empty> _checkAllButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _checkSelectedAddButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _exportAsJsonButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _exportAsTextButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<string> _refreshButtonClickedSubject = new Subject<string>();

        private AssetRegulationTestApplication _application;
        private GUIStyle _emptyLabelStyle;
        private bool _isSearchTextDirty;
        private DateTime _lastSearchFieldUpdateTime;
        private GUIStyle _placeholderStyle;
        private AssetRegulationViewerState _state;
        private SearchField _searchField;

        private GUIStyle PlaceholderStyle
        {
            get
            {
                if (_placeholderStyle == null)
                {
                    _placeholderStyle = new GUIStyle(EditorStyles.label);
                    _placeholderStyle.normal.textColor = Color.gray;
                }

                return _placeholderStyle;
            }
        }

        private GUIStyle EmptyLabelStyle
        {
            get
            {
                if (_emptyLabelStyle == null)
                {
                    _emptyLabelStyle = new GUIStyle(EditorStyles.label);
                    _emptyLabelStyle.wordWrap = true;
                    _emptyLabelStyle.alignment = TextAnchor.MiddleCenter;
                }

                return _emptyLabelStyle;
            }
        }

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
                _treeViewState = new AssetRegulationViewerTreeViewState();

            TreeView = new AssetRegulationViewerTreeView(_treeViewState);
            _searchField = new SearchField();
            _searchField.downOrUpArrowKeyPressed += TreeView.SetFocusAndEnsureSelectedItem;

            _application = AssetRegulationTestApplication.RequestInstance();
            _application.AssetRegulationViewerController.Setup(this);
            _application.AssetRegulationViewerPresenter.Setup(this);

            OnAssetPathOrFilterChanged();
            _isSearchTextDirty = false;

            if (string.IsNullOrEmpty(_searchText))
                _searchField.SetFocus();
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
                // Refresh Button
                var refreshButtonRect = GUILayoutUtility.GetRect(24, 20);
                var refreshButtonImageRect = refreshButtonRect;
                refreshButtonImageRect.xMin += 4;
                refreshButtonImageRect.xMax -= 4;
                refreshButtonImageRect.yMin += 2;
                refreshButtonImageRect.yMax -= 2;
                var refreshIconTexture = EditorGUIUtility.IconContent(EditorGUIUtil.RefreshIconName).image;
                GUI.DrawTexture(refreshButtonImageRect, refreshIconTexture, ScaleMode.StretchToFill);
                if (GUI.Button(refreshButtonRect, "", GUIStyle.none))
                    _refreshButtonClickedSubject.OnNext(_searchText);

                // Search Field.
                using (var ccs = new EditorGUI.ChangeCheckScope())
                {
                    _searchText = _searchField.OnToolbarGUI(_searchText, GUILayout.MaxWidth(1000));
                    if (string.IsNullOrEmpty(_searchText))
                    {
                        var placeholderRect = GUILayoutUtility.GetLastRect();
                        placeholderRect.xMin += 16;
                        placeholderRect.yMin -= 2;
                        EditorGUI.LabelField(placeholderRect, PlaceHolder, PlaceholderStyle);
                    }

                    if (ccs.changed)
                    {
                        _lastSearchFieldUpdateTime = DateTime.Now;
                        _isSearchTextDirty = true;
                    }
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Run All", EditorStyles.toolbarButton))
                    _checkAllButtonClickedSubject.OnNext(Empty.Default);

                if (GUILayout.Button("Run Selected", EditorStyles.toolbarButton))
                    _checkSelectedAddButtonClickedSubject.OnNext(Empty.Default);

                // Menu Button
                var menuButtonRect = GUILayoutUtility.GetRect(24, 20);
                var menuButtonImageRect = menuButtonRect;
                menuButtonImageRect.xMin += 2;
                menuButtonImageRect.xMax -= 2;
                var menuIconTexture = EditorGUIUtility.IconContent(EditorGUIUtil.MenuIconName).image;
                GUI.DrawTexture(menuButtonImageRect, menuIconTexture, ScaleMode.StretchToFill);
                if (GUI.Button(menuButtonRect, "", GUIStyle.none))
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Hide Empty"), ExcludeEmptyTests.Value,
                        () => ExcludeEmptyTests.Value = !ExcludeEmptyTests.Value);
                    menu.AddItem(new GUIContent("Export/Text"), false,
                        () => _exportAsTextButtonClickedSubject.OnNext(Empty.Default));
                    menu.AddItem(new GUIContent("Export/Json"), false,
                        () => _exportAsJsonButtonClickedSubject.OnNext(Empty.Default));

                    menu.ShowAsContext();
                }
            }

            if (string.IsNullOrEmpty(_searchText))
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical(GUILayout.Width(position.width - 100));
                GUILayout.FlexibleSpace();
                GUILayout.Label("Enter the target asset name in the search field.", EmptyLabelStyle);
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else
            {
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

                TreeView.OnGUI(treeViewRect);
            }
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
            if (string.IsNullOrEmpty(assetPath)) return;

            var assetName = Path.GetFileNameWithoutExtension(assetPath);
            Open(assetName);
        }
    }
}

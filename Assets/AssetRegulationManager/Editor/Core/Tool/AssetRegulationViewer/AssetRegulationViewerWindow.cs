// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Foundation.Observable;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationViewer
{
    internal sealed class AssetRegulationViewerWindow : EditorWindow
    {
        [SerializeField] private TreeViewState _treeViewState;
        [SerializeField] private string _searchText;

        private readonly Subject<string> _assetPathOrFilterSubject = new Subject<string>();
        private readonly Subject<Empty> _checkAllButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _checkSelectedAddButtonClickedSubject = new Subject<Empty>();
        private AssetRegulationViewerApplication _application;
        private bool _displayedTreeView;
        private SearchField _searchField;

        internal IObservable<string> AssetPathOrFilterObservable => _assetPathOrFilterSubject;
        internal IObservable<Empty> CheckAllButtonClickedObservable => _checkAllButtonClickedSubject;
        internal IObservable<Empty> CheckSelectedAddButtonClickedObservable => _checkSelectedAddButtonClickedSubject;
        internal AssetRegulationTreeView TreeView { get; private set; }

        private void OnEnable()
        {
            if (_treeViewState == null)
            {
                _treeViewState = new TreeViewState();
            }

            // Create TreeView
            TreeView = new AssetRegulationTreeView(_treeViewState);

            _searchField = new SearchField();
            _searchField.downOrUpArrowKeyPressed += TreeView.SetFocusAndEnsureSelectedItem;

            _displayedTreeView = !string.IsNullOrEmpty(_searchText);

            // Instance Setup
            _application = AssetRegulationViewerApplication.RequestInstance();
            _application.AssetRegulationViewerController.Setup(this);
            _application.AssetRegulationViewerPresenter.Setup(this);

            if (_displayedTreeView)
            {
                _assetPathOrFilterSubject.OnNext(_searchText);
            }
        }

        private void OnDisable()
        {
            _searchField.downOrUpArrowKeyPressed -= TreeView.SetFocusAndEnsureSelectedItem;
            AssetRegulationViewerApplication.ReleaseInstance();
        }

        private void OnGUI()
        {
            // Toolbar
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Space(4);
                _searchText = _searchField.OnToolbarGUI(_searchText);
                if (GUILayout.Button("Search Assets", EditorStyles.toolbarButton))
                {
                    _displayedTreeView = !string.IsNullOrEmpty(_searchText);
                    if (_displayedTreeView)
                    {
                        _assetPathOrFilterSubject.OnNext(_searchText);
                    }
                }

                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Check All", EditorStyles.toolbarButton))
                {
                    _checkAllButtonClickedSubject.OnNext(Empty.Default);
                }

                if (GUILayout.Button("Check Selected", EditorStyles.toolbarButton))
                {
                    _checkSelectedAddButtonClickedSubject.OnNext(Empty.Default);
                }
            }

            // Draw Help Box
            if (!_displayedTreeView)
            {
                EditorGUILayout.HelpBox("Enter the asset path and click Search Assets to search for asset regulations",
                    MessageType.Info);
                return;
            }

            // Draw Tree View
            var treeViewRect = GUILayoutUtility.GetRect(0, float.MaxValue, 0, float.MaxValue);

            TreeView.Reload();
            TreeView.OnGUI(treeViewRect);
        }

        [MenuItem("Window/Asset Regulation Viewer")]
        private static void ShowWindow()
        {
            GetWindow<AssetRegulationViewerWindow>();
        }
    }
}

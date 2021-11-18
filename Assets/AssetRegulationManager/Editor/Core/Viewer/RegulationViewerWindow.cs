// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Foundation.Observable;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal class RegulationViewerWindow : EditorWindow
    {
        [SerializeField] private TreeViewState _treeViewState;
        [SerializeField] private string _searchText;

        internal IObservable<string> SearchAssetButtonClickedObservable => _searchAssetButtonClickedSubject;
        internal IObservable<Empty> CheckAllButtonClickedObservable => _checkAllButtonClickedSubject;
        internal IObservable<Empty> CheckSelectedAddButtonClickedObservable => _checkSelectedAddButtonClickedSubject;
        internal RegulationTreeView TreeView => _treeView;

        private readonly Subject<string> _searchAssetButtonClickedSubject = new Subject<string>();
        private readonly Subject<Empty> _checkAllButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _checkSelectedAddButtonClickedSubject = new Subject<Empty>();
        private bool _displayedTreeView;
        private SearchField _searchField;
        private RegulationTreeView _treeView;
        private RegulationViewerApplication _application;
        private Rect _treeViewRect;

        private void OnEnable()
        {
            if (_treeViewState == null) _treeViewState = new TreeViewState();
            
            // Create TreeView
            _treeView = new RegulationTreeView(_treeViewState);
            
            _searchField = new SearchField();
            _searchField.downOrUpArrowKeyPressed += _treeView.SetFocusAndEnsureSelectedItem;

            _displayedTreeView = !string.IsNullOrEmpty(_searchText);
            
            _treeViewRect = GUILayoutUtility.GetRect(0, float.MaxValue, 0, float.MaxValue);
            
            // Instance Setup
            _application = RegulationViewerApplication.RequestInstance();
            _application.RegulationViewerController.Setup(this);
            _application.RegulationViewerPresenter.Setup(this);
            
            if(_displayedTreeView)
                _searchAssetButtonClickedSubject.OnNext(_searchText);
        }

        private void OnDisable()
        {
            _searchField.downOrUpArrowKeyPressed -= _treeView.SetFocusAndEnsureSelectedItem;
            RegulationViewerApplication.ReleaseInstance();
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
                    if(_displayedTreeView)
                        _searchAssetButtonClickedSubject.OnNext(_searchText);
                }

                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Check All", EditorStyles.toolbarButton)) _checkAllButtonClickedSubject.OnNext(Empty.Default);
                if (GUILayout.Button("Check Selected", EditorStyles.toolbarButton)) _checkSelectedAddButtonClickedSubject.OnNext(Empty.Default);
            }
            
            // Draw Help Box
            if (!_displayedTreeView)
            {
                EditorGUILayout.HelpBox("Enter the asset path and click Search Assets to search for regulations",
                    MessageType.Info);
                return;
            }

            // Draw Tree View
            _treeView.Reload();
            _treeView.OnGUI(_treeViewRect);
        }

        [MenuItem("Window/Regulation Viewer")]
        private static void ShowWindow()
        {
            GetWindow<RegulationViewerWindow>();
        }
    }
}
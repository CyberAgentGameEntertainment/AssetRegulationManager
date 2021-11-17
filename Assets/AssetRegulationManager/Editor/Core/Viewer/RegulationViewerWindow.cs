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
    public class RegulationViewerWindow : EditorWindow
    {
        [SerializeField] private TreeViewState _treeViewState;
        [SerializeField] private string _searchText;

        public IObservable<string> SearchAssetButtonClickedObservable => _searchAssetButtonClickedSubject;
        public IObservable<Empty> CheckAllButtonClickedObservable => _checkAllButtonClickedSubject;
        public IObservable<Empty> CheckSelectedAddButtonClickedObservable => _checkSelectedAddButtonClickedSubject;

        private readonly Subject<string> _searchAssetButtonClickedSubject = new Subject<string>();
        private readonly Subject<Empty> _checkAllButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _checkSelectedAddButtonClickedSubject = new Subject<Empty>();
        private bool _displayedTreeView;
        private SearchField _searchField;
        private RegulationTreeView _treeView;
        private RegulationViewerApplication _application;

        private void OnEnable()
        {
            if (_treeViewState == null) _treeViewState = new TreeViewState();

            // TreeViewを作成
            _treeView = new RegulationTreeView(_treeViewState);

            _searchField = new SearchField();
            _searchField.downOrUpArrowKeyPressed += _treeView.SetFocusAndEnsureSelectedItem;

            _displayedTreeView = !string.IsNullOrEmpty(_searchText);
            
            _application = RegulationViewerApplication.RequestInstance();
            _application.RegulationViewerController.Setup(this);
            _application.RegulationViewerPresenter.Setup(this);
            
            if(!string.IsNullOrEmpty(_searchText))
                _searchAssetButtonClickedSubject.OnNext(_searchText);
        }

        private void OnDisable()
        {
            _searchField.downOrUpArrowKeyPressed -= _treeView.SetFocusAndEnsureSelectedItem;
            RegulationViewerApplication.ReleaseInstance();
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Space(4);
                _searchText = _searchField.OnToolbarGUI(_searchText);
                if (GUILayout.Button("Search Assets", EditorStyles.toolbarButton))
                {
                    _displayedTreeView = !string.IsNullOrEmpty(_searchText);
                    if(!string.IsNullOrEmpty(_searchText))
                        _searchAssetButtonClickedSubject.OnNext(_searchText);
                }

                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Check All", EditorStyles.toolbarButton)) _checkAllButtonClickedSubject.OnNext(Empty.Default);
                if (GUILayout.Button("Check Selected", EditorStyles.toolbarButton)) _checkSelectedAddButtonClickedSubject.OnNext(Empty.Default);
            }

            if (!_displayedTreeView)
            {
                EditorGUILayout.HelpBox("Enter the asset path and click Search Assets to search for regulations",
                    MessageType.Info);
                return;
            }

            _treeView.Reload();
            var rect = GUILayoutUtility.GetRect(0, float.MaxValue, 0, float.MaxValue);
            _treeView.OnGUI(rect);
        }

        [MenuItem("Window/Regulation Viewer")]
        private static void ShowWindow()
        {
            GetWindow<RegulationViewerWindow>();
        }
    }
}
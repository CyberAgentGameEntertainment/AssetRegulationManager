using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;

namespace AssetRegulationManager.Editor
{
    public class RegulationViewerWindow : EditorWindow
    {
        [SerializeField]
        private TreeViewState _treeViewState;
        [SerializeField]
        private RegulationTreeView _treeView;
        [SerializeField]
        private SearchField _searchField;
        
        private string _searchText;
        private bool _displayedTreeView;

        [MenuItem("Window/Regulation Viewer")]
        private static void ShowWindow()
        {
            GetWindow<RegulationViewerWindow>();
        }

        private void OnEnable()
        {
            // Stateは生成されていたらそれを使う
            if (_treeViewState == null) {
                _treeViewState = new TreeViewState ();
            }

            // TreeViewを作成
            _treeView = new RegulationTreeView(_treeViewState);

            _searchField = new SearchField();
            _searchField.downOrUpArrowKeyPressed += _treeView.SetFocusAndEnsureSelectedItem;

            _displayedTreeView = !string.IsNullOrEmpty(_searchText);
            if (_displayedTreeView)
            {
                SearchAssetsToTreeView(_searchText);
            }
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
                    if (_displayedTreeView)
                    {
                        SearchAssetsToTreeView(_searchText);
                    }
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Check All", EditorStyles.toolbarButton))
                {
                    Debug.Log("Check All");
                }
                if (GUILayout.Button("Check Selected", EditorStyles.toolbarButton))
                {
                    Debug.Log("Check Selected");
                }
            }

            if (!_displayedTreeView)
            {
                EditorGUILayout.HelpBox("Enter the asset path and click Search Assets to search for regulations", MessageType.Info);
                return;
            }
            _treeView.Reload();
            var rect = GUILayoutUtility.GetRect(0, float.MaxValue, 0, float.MaxValue);
            _treeView.OnGUI(rect);
        }

        public void OnDisable()
        {
            _searchField.downOrUpArrowKeyPressed -= _treeView.SetFocusAndEnsureSelectedItem;
        }

        private void SearchAssetsToTreeView(string searchText)
        {
            _treeView.ClearItems();
            var currentId = 0;
                    
            // 検索文字列から検索しPathに変換
            foreach (var path in AssetDatabase.FindAssets(searchText).Select(AssetDatabase.GUIDToAssetPath))
            {
                var parentId = ++currentId;
                _treeView.AddItemAndSetParent(new TreeViewItem(){ id = parentId, displayName = path}, -1);
                // TODO: SubAssetの表示に切り替える
                _treeView.AddItemAndSetParent(new TreeViewItem(){ id = ++currentId, displayName = "1"}, parentId);
                _treeView.AddItemAndSetParent(new TreeViewItem(){ id = ++currentId, displayName = "2"}, parentId);
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetsRegulation
{
    public class RegulationViewerWindow : EditorWindow
    {
        [SerializeField]
        private TreeViewState _treeViewState;
        
        private RegulationTreeView _treeView;
        private SearchField _searchField;
        private string _searchText;

        [MenuItem("Window/Regulation Viewer")]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow<RegulationViewerWindow>();
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
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                _searchText = _searchField.OnToolbarGUI(_searchText);
                if (GUILayout.Button("Search Assets", EditorStyles.toolbarButton))
                {
                    _treeView.ClearItems();
                    var currentId = 0;
                    
                    // 検索文字列から検索しPathに変換
                    foreach (var path in AssetDatabase.FindAssets(_searchText).Select(AssetDatabase.GUIDToAssetPath))
                    {
                        var parentId = ++currentId;
                        _treeView.AddItemAndSetParent(new TreeViewItem(){ id = parentId, displayName = path}, -1);
                        // TODO: SubAssetの表示に切り替える
                        _treeView.AddItemAndSetParent(new TreeViewItem(){ id = ++currentId, displayName = "1"}, parentId);
                        _treeView.AddItemAndSetParent(new TreeViewItem(){ id = ++currentId, displayName = "2"}, parentId);
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
            
            _treeView.Reload();
            var rect = GUILayoutUtility.GetRect(0, float.MaxValue, 0, float.MaxValue);
            _treeView.OnGUI(rect);
        }
    }
}

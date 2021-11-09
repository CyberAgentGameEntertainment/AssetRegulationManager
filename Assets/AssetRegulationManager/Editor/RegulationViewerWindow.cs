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
                    var currentId = 0;
                    var elements = new List<RegulationTreeElement>();
                    
                     // 検索文字列から検索しPathに変換
                    foreach (var path in AssetDatabase.FindAssets(_searchText).Select(AssetDatabase.GUIDToAssetPath))
                    {
                        var root = new RegulationTreeElement {Id = ++currentId, Name = path};
                        // ダミーの子を作成
                        for (int i = 0; i < 2; i++) {
                            var element = new RegulationTreeElement { Id = ++currentId, Name = "1-" + (i + 1) };
                            root.AddChild(element);
                        }
                        elements.Add(root);
                    }
                    
                    _treeView.Setup(elements.ToArray());
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
            
            var rect = GUILayoutUtility.GetRect(0, float.MaxValue, 0, float.MaxValue);
            _treeView.OnGUI(rect);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Shared;
using AssetRegulationManager.Editor.Foundation;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using AssetRegulationManager.Editor.Foundation.OrderCollections;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetGroupView : IDisposable
    {
        private const string RenameGroupMenuName = "Rename";
        private const string RemoveGroupMenuName = "Remove";
        private const string MoveUpMenuName = "Move Up";
        private const string MoveDownMenuName = "Move Down";
        private const string RemoveFilterMenuName = "Remove Filter";
        private const string MoveUpFilterMenuName = "Move Up Filter";
        private const string MoveDownFilterMenuName = "Move Down Filter";

        private readonly Subject<Empty> _addFilterButtonClickedSubject = new Subject<Empty>();

        private readonly Dictionary<string, ICustomDrawer> _filterDrawers = new Dictionary<string, ICustomDrawer>();

        private readonly ObservableDictionary<string, IAssetFilter> _filters =
            new ObservableDictionary<string, IAssetFilter>();

        private readonly Subject<string> _filterValueChangedSubject = new Subject<string>();
        private readonly ObservableProperty<string> _name = new ObservableProperty<string>();
        private readonly Subject<string> _removeFilterMenuExecutedSubject = new Subject<string>();
        private readonly Subject<string> _moveUpFilterMenuExecutedSubject = new Subject<string>();
        private readonly Subject<string> _moveDownFilterMenuExecutedSubject = new Subject<string>();
        private readonly Subject<Empty> _removeGroupMenuExecutedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _moveUpMenuExecutedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _moveDownMenuExecutedSubject = new Subject<Empty>();
        private readonly StringOrderCollection _filterOrders = new StringOrderCollection();

        public AssetGroupView(string assetGroupId)
        {
            AssetGroupId = assetGroupId;
        }

        public IObservableProperty<string> Name => _name;

        public IReadOnlyObservableDictionary<string, IAssetFilter> Filters => _filters;

        public IObservable<Empty> RemoveGroupMenuExecutedAsObservable => _removeGroupMenuExecutedSubject;

        public IObservable<Empty> MoveUpMenuExecutedAsObservable => _moveUpMenuExecutedSubject;

        public IObservable<Empty> MoveDownMenuExecutedObservable => _moveDownMenuExecutedSubject;

        public IObservable<string> RemoveFilterMenuExecutedAsObservable => _removeFilterMenuExecutedSubject;

        public IObservable<string> MoveUpFilterMenuExecutedAsObservable => _moveUpFilterMenuExecutedSubject;

        public IObservable<string> MoveDownFilterMenuExecutedAsObservable => _moveDownFilterMenuExecutedSubject;

        public string AssetGroupId { get; }

        public IObservable<Empty> AddFilterButtonClickedAsObservable => _addFilterButtonClickedSubject;

        public IObservable<string> FilterValueChangedAsObservable => _filterValueChangedSubject;

        public void Dispose()
        {
            _filters.Dispose();
            _name.Dispose();
            _addFilterButtonClickedSubject.Dispose();
            _filterValueChangedSubject.Dispose();
            _removeFilterMenuExecutedSubject.Dispose();
            _moveUpMenuExecutedSubject.Dispose();
            _moveDownMenuExecutedSubject.Dispose();
            _removeGroupMenuExecutedSubject.Dispose();
            _moveUpMenuExecutedSubject.Dispose();
            _moveDownMenuExecutedSubject.Dispose();
        }

        public void AddFilter(IAssetFilter filter)
        {
            _filterOrders.Add(filter.Id);
            _filters.Add(filter.Id, filter);
            var drawer = CustomDrawerFactory.Create(filter.GetType());

            if (drawer == null)
            {
                Debug.LogError($"Drawer of {filter.GetType().Name} is not found.");
                return;
            }

            drawer.Setup(filter);
            _filterDrawers.Add(filter.Id, drawer);
        }

        public void RemoveFilter(string id)
        {
            _filters.Remove(id);
            _filterDrawers.Remove(id);
            _filters.Remove(id);
        }

        public void ClearFilters()
        {
            _filters.Clear();
            _filterDrawers.Clear();
            _filterOrders.Clear();
        }

        public void SetFilterOrder(string id, int index)
        {
            _filterOrders.SetIndex(id, index);
        }

        public void DoLayout()
        {
            var rect = GUILayoutUtility.GetRect(1, 20, GUILayout.ExpandWidth(true));

            // Title
            EditorGUI.DrawRect(rect, EditorGUIUtil.TitleBackgroundColor);
            rect.xMin += 4;
            var titleRect = rect;
            titleRect.xMax -= 20;
            EditorGUI.LabelField(rect, _name.Value, EditorStyles.boldLabel);
            var titleButtonRect = rect;
            titleButtonRect.xMin += titleButtonRect.width - 20;

            var plusIconTexture = EditorGUIUtility.IconContent(EditorGUIUtil.ToolbarPlusIconName).image;
            GUI.DrawTexture(titleButtonRect, plusIconTexture, ScaleMode.StretchToFill);
            if (GUI.Button(titleButtonRect, "", GUIStyle.none))
                _addFilterButtonClickedSubject.OnNext(Empty.Default);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(4);
            EditorGUILayout.BeginVertical();

            // Right Click Menu of Title
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 &&
                titleRect.Contains(Event.current.mousePosition))
            {
                var menu = new GenericMenu();
                var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                menu.AddItem(new GUIContent(RenameGroupMenuName), false,
                    () =>
                    {
                        TextFieldPopup.Show(mousePos, _name.Value, x => _name.Value = x, null,
                            RenameGroupMenuName);
                    });
                menu.AddItem(new GUIContent(RemoveGroupMenuName), false,
                    () => _removeGroupMenuExecutedSubject.OnNext(Empty.Default));
                menu.AddItem(new GUIContent(MoveUpMenuName), false,
                    () => _moveUpMenuExecutedSubject.OnNext(Empty.Default));
                menu.AddItem(new GUIContent(MoveDownMenuName), false,
                    () => _moveDownMenuExecutedSubject.OnNext(Empty.Default));
                menu.ShowAsContext();
            }

            GUILayout.Space(8);

            // Filters
            foreach (var filter in _filters.Values.OrderBy(x => _filterOrders.GetIndex(x.Id)))
            {
                EditorGUILayout.BeginVertical("Box");

                var filterTitleRect = GUILayoutUtility.GetRect(1, 16, GUILayout.ExpandWidth(true));
                var attribute = filter.GetType().GetCustomAttribute<AssetFilterAttribute>();
                var filterTitle = attribute == null
                    ? ObjectNames.NicifyVariableName(filter.GetType().Name)
                    : attribute.DisplayName;
                EditorGUI.LabelField(filterTitleRect, filterTitle, EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(12);
                EditorGUILayout.BeginVertical();

                if (!_filterDrawers.TryGetValue(filter.Id, out var drawer))
                    EditorGUILayout.LabelField($"[{filter.GetType().Name}] Drawer is not found.");
                else
                    using (var ccs = new EditorGUI.ChangeCheckScope())
                    {
                        drawer.DoLayout();

                        if (ccs.changed)
                            _filterValueChangedSubject.OnNext(filter.Id);
                    }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                // Right Click Menu of Filter
                if (Event.current.type == EventType.MouseDown && Event.current.button == 1
                                                              && filterTitleRect.Contains(Event.current.mousePosition))
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent(RemoveFilterMenuName), false,
                        () => _removeFilterMenuExecutedSubject.OnNext(filter.Id));
                    menu.AddItem(new GUIContent(MoveUpFilterMenuName), false,
                        () => _moveUpFilterMenuExecutedSubject.OnNext(filter.Id));
                    menu.AddItem(new GUIContent(MoveDownFilterMenuName), false,
                        () => _moveDownFilterMenuExecutedSubject.OnNext(filter.Id));
                    menu.ShowAsContext();
                }

                GUILayout.Space(2);
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(4);

            // Border
            var borderRect = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(borderRect, EditorGUIUtil.EditorBorderColor);
        }
    }
}

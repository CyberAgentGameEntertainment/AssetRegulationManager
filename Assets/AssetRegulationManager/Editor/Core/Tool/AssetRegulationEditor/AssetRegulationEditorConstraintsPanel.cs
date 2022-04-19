using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Shared;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using AssetRegulationManager.Editor.Foundation.OrderCollections;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorConstraintsPanel : IDisposable
    {
        private const string RemoveMenuName = "Remove";
        private const string MoveUpMenuName = "Move Up";
        private const string MoveDownMenuName = "Move Down";
        private const string CopyMenuName = "Copy";
        private const string PasteMenuName = "Paste As New";
        private const string PasteValuesMenuName = "Paste Values";
        private const string AddConstraintButtonName = "Add Constraint";
        private const string PasteConstraintButtonName = "Paste Constraint";
        
        private readonly Subject<Empty> _addConstraintButtonClickedSubject = new Subject<Empty>();
        private readonly Subject<Empty> _pasteConstraintMenuExecutedSubject = new Subject<Empty>();

        private readonly Dictionary<string, ICustomDrawer> _constraintDrawers = new Dictionary<string, ICustomDrawer>();
        private readonly StringOrderCollection _constraintOrders = new StringOrderCollection();

        private readonly ObservableDictionary<string, IAssetConstraint> _constraints =
            new ObservableDictionary<string, IAssetConstraint>();

        private readonly Subject<string> _constraintValueChangedSubject = new Subject<string>();
        private readonly Subject<string> _copyMenuExecutedSubject = new Subject<string>();

        private readonly Subject<Empty> _mouseDownSubject = new Subject<Empty>();
        private readonly Subject<string> _moveDownMenuExecutedSubject = new Subject<string>();
        private readonly Subject<string> _moveUpMenuExecutedSubject = new Subject<string>();
        private readonly Subject<Empty> _pasteMenuExecutedSubject = new Subject<Empty>();
        private readonly Subject<string> _pasteValuesMenuExecutedSubject = new Subject<string>();
        private readonly Subject<string> _removeConstraintMenuExecutedSubject = new Subject<string>();

        private Func<bool> _canPaste;
        private Func<string, bool> _canPasteValues;

        public IReadOnlyObservableDictionary<string, IAssetConstraint> Constraints => _constraints;

        public IObservable<Empty> MouseDownAsObservable => _mouseDownSubject;

        public IObservable<string> RemoveConstraintMenuExecutedAsObservable => _removeConstraintMenuExecutedSubject;

        public IObservable<Empty> AddConstraintButtonClickedAsObservable => _addConstraintButtonClickedSubject;

        public IObservable<Empty> PasteConstraintMenuExecutedAsObservable => _pasteConstraintMenuExecutedSubject;

        public IObservable<string> ConstraintValueChangedAsObservable => _constraintValueChangedSubject;

        public IObservable<string> MoveUpMenuExecutedAsObservable => _moveUpMenuExecutedSubject;

        public IObservable<string> MoveDownMenuExecutedObservable => _moveDownMenuExecutedSubject;

        public IObservable<string> CopyMenuExecutedAsObservable => _copyMenuExecutedSubject;

        public IObservable<Empty> PasteMenuExecutedSubject => _pasteMenuExecutedSubject;

        public IObservable<string> PasteValuesMenuExecutedSubject => _pasteValuesMenuExecutedSubject;

        public bool Enabled { get; set; }

        public void Dispose()
        {
            _constraints.Dispose();
            _addConstraintButtonClickedSubject.Dispose();
            _pasteConstraintMenuExecutedSubject.Dispose();
            _constraintValueChangedSubject.Dispose();
            _removeConstraintMenuExecutedSubject.Dispose();
            _moveUpMenuExecutedSubject.Dispose();
            _moveDownMenuExecutedSubject.Dispose();
            _copyMenuExecutedSubject.Dispose();
            _pasteMenuExecutedSubject.Dispose();
            _pasteValuesMenuExecutedSubject.Dispose();
            _mouseDownSubject.Dispose();
        }

        public void SetupClipboard(Func<bool> canPaste, Func<string, bool> canPasteValues)
        {
            _canPaste = canPaste;
            _canPasteValues = canPasteValues;
        }

        public void AddConstraint(IAssetConstraint constraint)
        {
            _constraints.Add(constraint.Id, constraint);
            var drawer = CustomDrawerFactory.Create(constraint.GetType());

            if (drawer == null)
            {
                Debug.LogError($"Drawer of {constraint.GetType().Name} is not found.");
                return;
            }

            drawer.Setup(constraint);
            _constraintOrders.Add(constraint.Id);
            _constraintDrawers.Add(constraint.Id, drawer);
        }

        public void RemoveConstraint(string id)
        {
            _constraints.Remove(id);
            _constraintDrawers.Remove(id);
            _constraintOrders.Remove(id);
        }

        public void ClearConstraints()
        {
            _constraints.Clear();
            _constraintDrawers.Clear();
            _constraintOrders.Clear();
        }

        public void SetConstraintOrder(string id, int index)
        {
            _constraintOrders.SetIndex(id, index);
        }

        public void DoLayout()
        {
            if (!Enabled)
                return;

            if (Event.current.type == EventType.MouseDown)
                _mouseDownSubject.OnNext(Empty.Default);

            // Constraints
            foreach (var constraint in _constraints.Values.OrderBy(x => _constraintOrders.GetIndex(x.Id)))
            {
                // Draw Title
                var titleRect = GUILayoutUtility.GetRect(1, 20, GUILayout.ExpandWidth(true));
                EditorGUI.DrawRect(titleRect, EditorGUIUtil.TitleBackgroundColor);
                titleRect.xMin += 4;
                var attribute = constraint.GetType().GetCustomAttribute<AssetConstraintAttribute>();
                var title = attribute == null
                    ? ObjectNames.NicifyVariableName(constraint.GetType().Name)
                    : attribute.DisplayName;
                EditorGUI.LabelField(titleRect, title, EditorStyles.boldLabel);

                GUILayout.Space(4);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(16);
                EditorGUILayout.BeginVertical();

                // Draw Constraint
                if (!_constraintDrawers.TryGetValue(constraint.Id, out var drawer))
                    EditorGUILayout.LabelField("Drawer is not found.");
                else
                    using (var ccs = new EditorGUI.ChangeCheckScope())
                    {
                        drawer.DoLayout();

                        if (ccs.changed)
                            _constraintValueChangedSubject.OnNext(constraint.Id);
                    }

                // Right Click Menu of Constraint
                if (Event.current.type == EventType.MouseDown && Event.current.button == 1 &&
                    titleRect.Contains(Event.current.mousePosition))
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent(RemoveMenuName), false,
                        () => _removeConstraintMenuExecutedSubject.OnNext(constraint.Id));
                    menu.AddItem(new GUIContent(MoveUpMenuName), false,
                        () => _moveUpMenuExecutedSubject.OnNext(constraint.Id));
                    menu.AddItem(new GUIContent(MoveDownMenuName), false,
                        () => _moveDownMenuExecutedSubject.OnNext(constraint.Id));
                    menu.AddItem(new GUIContent(CopyMenuName), false,
                        () => _copyMenuExecutedSubject.OnNext(constraint.Id));

                    // Paste
                    if (_canPaste.Invoke())
                        menu.AddItem(new GUIContent(PasteMenuName), false,
                            () => _pasteMenuExecutedSubject.OnNext(Empty.Default));
                    else
                        menu.AddDisabledItem(new GUIContent(PasteMenuName), false);

                    // Paste Values
                    if (_canPasteValues.Invoke(constraint.Id))
                        menu.AddItem(new GUIContent(PasteValuesMenuName), false,
                            () => _pasteValuesMenuExecutedSubject.OnNext(constraint.Id));
                    else
                        menu.AddDisabledItem(new GUIContent(PasteValuesMenuName), false);

                    menu.ShowAsContext();
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(8);

                // Border
                var borderRect = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true));
                EditorGUI.DrawRect(borderRect, EditorGUIUtil.EditorBorderColor);
            }

            var bottomRect = GUILayoutUtility.GetRect(1, EditorGUIUtility.singleLineHeight + 8,
                GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            var buttonRect = bottomRect;
            buttonRect.height = EditorGUIUtility.singleLineHeight;
            buttonRect.y += 4;
            buttonRect.x = buttonRect.width / 2.0f - 60;
            buttonRect.width = 120;
            if (GUI.Button(buttonRect, AddConstraintButtonName))
                _addConstraintButtonClickedSubject.OnNext(Empty.Default);

            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 &&
                bottomRect.Contains(Event.current.mousePosition))
            {
                var menu = new GenericMenu();

                if (_canPaste.Invoke())
                    menu.AddItem(new GUIContent(PasteConstraintButtonName), false,
                        () => _pasteConstraintMenuExecutedSubject.OnNext(Empty.Default));
                else
                    menu.AddDisabledItem(new GUIContent(PasteConstraintButtonName), false);

                menu.ShowAsContext();
            }
        }
    }
}

// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Foundation.SelectableSerializeReference
{
    [CustomPropertyDrawer(typeof(SelectableSerializeReferenceAttribute))]
    public sealed class SelectableSerializeReferenceAttributeDrawer : PropertyDrawer
    {
        private readonly Dictionary<string, PropertyData> _dataPerPath =
            new Dictionary<string, PropertyData>();

        private PropertyData _data;

        private int _selectedIndex;

        private void Init(SerializedProperty property)
        {
            if (_dataPerPath.TryGetValue(property.propertyPath, out _data))
            {
                return;
            }

            _data = new PropertyData(property);
            _dataPerPath[property.propertyPath] = _data;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = (SelectableSerializeReferenceAttribute)attribute;
            Assert.IsTrue(property.propertyType == SerializedPropertyType.ManagedReference);

            Init(property);

            var fullTypeName = property.managedReferenceFullTypename.Split(' ').Last();
            _selectedIndex = Array.IndexOf(_data.DerivedFullTypeNames, fullTypeName) + 1;

            position.y += EditorGUIUtility.standardVerticalSpacing;
            var labelPosition = position;
            labelPosition.height = EditorGUIUtility.singleLineHeight;
            using (var ccs = new EditorGUI.ChangeCheckScope())
            {
                var selectorPosition = position;

                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                selectorPosition.width -= EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing;
                selectorPosition.x += EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing;
                selectorPosition.height = EditorGUIUtility.singleLineHeight;
                var selectedIndex = EditorGUI.Popup(selectorPosition, _selectedIndex, _data.Options);
                if (ccs.changed && _selectedIndex != selectedIndex)
                {
                    _selectedIndex = selectedIndex;
                    var selectedType = selectedIndex == 0 ? null : _data.DerivedTypes[selectedIndex - 1];
                    property.managedReferenceValue =
                        selectedType == null ? null : Activator.CreateInstance(selectedType);
                }

                labelPosition.xMax -= selectorPosition.width;

                EditorGUI.indentLevel = indent;
            }

            EditorGUI.PropertyField(position, property, new GUIContent(string.Empty), true);

            if (attr.LabelType == LabelType.ClassName && _selectedIndex >= 1)
            {
                label.text = _data.Options[_selectedIndex];
            }
            else if (attr.LabelType == LabelType.PropertyName)
            {
                label.text = property.displayName;
            }

            // If the property is a array element, add offset.
            if (property.propertyPath.EndsWith("]"))
            {
                labelPosition.xMin += 13;
            }

            EditorGUI.LabelField(labelPosition, label, EditorStyles.boldLabel);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property);

            if (string.IsNullOrEmpty(property.managedReferenceFullTypename))
            {
                return EditorGUIUtility.singleLineHeight;
            }

            var height = 0.0f;
            height += EditorGUIUtility.standardVerticalSpacing;
            height += EditorGUI.GetPropertyHeight(property, true);
            return height;
        }

        private class PropertyData
        {
            public PropertyData(SerializedProperty property)
            {
                var managedReferenceFieldTypenameSplit = property.managedReferenceFieldTypename.Split(' ').ToArray();
                var assemblyName = managedReferenceFieldTypenameSplit[0];
                var fieldTypeName = managedReferenceFieldTypenameSplit[1];
                var fieldType = GetAssembly(assemblyName).GetType(fieldTypeName);
                DerivedTypes = TypeCache.GetTypesDerivedFrom(fieldType).Where(x =>
                    {
                        var isTestAssembly = x.Assembly.FullName.Contains(".Tests.");
                        return !x.IsAbstract && !x.IsInterface && !isTestAssembly;
                    })
                    .ToArray();
                DerivedTypeNames = new string[DerivedTypes.Length];
                DerivedFullTypeNames = new string[DerivedTypes.Length];
                Options = new string[DerivedTypes.Length + 1];
                Options[0] = $"None ({fieldType.Name})";
                for (var i = 0; i < DerivedTypes.Length; i++)
                {
                    var type = DerivedTypes[i];
                    var isTarget = type.GetCustomAttribute<IgnoreSelectableSerializeReferenceAttribute>() == null;
                    if (!isTarget)
                    {
                        continue;
                    }

                    var label = type.GetCustomAttribute<SelectableSerializeReferenceLabelAttribute>()?.Label;
                    DerivedTypeNames[i] = type.Name;
                    DerivedFullTypeNames[i] = type.FullName;
                    Options[i + 1] = label ?? ObjectNames.NicifyVariableName(type.Name);
                }
            }

            public Type[] DerivedTypes { get; }

            public string[] DerivedTypeNames { get; }

            public string[] DerivedFullTypeNames { get; }

            public string[] Options { get; }

            private static Assembly GetAssembly(string name)
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(assembly => assembly.GetName().Name == name);
            }
        }
    }
}

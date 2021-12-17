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
    internal sealed class SelectableSerializeReferenceAttributeDrawer : PropertyDrawer
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
            _dataPerPath.Add(property.propertyPath, _data);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Assert.IsTrue(property.propertyType == SerializedPropertyType.ManagedReference);

            Init(property);

            var fullTypeName = property.managedReferenceFullTypename.Split(' ').Last();
            _selectedIndex = Array.IndexOf(_data.DerivedFullTypeNames, fullTypeName);

            using (var ccs = new EditorGUI.ChangeCheckScope())
            {
                var selectorPosition = position;

                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                selectorPosition.width -= EditorGUIUtility.labelWidth;
                selectorPosition.x += EditorGUIUtility.labelWidth;
                selectorPosition.height = EditorGUIUtility.singleLineHeight;
                var selectedTypeIndex = EditorGUI.Popup(selectorPosition, _selectedIndex, _data.DerivedTypeNames);
                if (ccs.changed)
                {
                    _selectedIndex = selectedTypeIndex;
                    var selectedType = _data.DerivedTypes[selectedTypeIndex];
                    property.managedReferenceValue =
                        selectedType == null ? null : Activator.CreateInstance(selectedType);
                }

                EditorGUI.indentLevel = indent;
            }

            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property);

            if (string.IsNullOrEmpty(property.managedReferenceFullTypename))
            {
                return EditorGUIUtility.singleLineHeight;
            }

            return EditorGUI.GetPropertyHeight(property, true);
        }

        private class PropertyData
        {
            public PropertyData(SerializedProperty property)
            {
                var managedReferenceFieldTypenameSplit = property.managedReferenceFieldTypename.Split(' ').ToArray();
                var assemblyName = managedReferenceFieldTypenameSplit[0];
                var fieldTypeName = managedReferenceFieldTypenameSplit[1];
                var fieldType = GetAssembly(assemblyName).GetType(fieldTypeName);
                DerivedTypes = TypeCache.GetTypesDerivedFrom(fieldType).Where(x => !x.IsAbstract && !x.IsInterface)
                    .ToArray();
                DerivedTypeNames = new string[DerivedTypes.Length];
                DerivedFullTypeNames = new string[DerivedTypes.Length];
                for (var i = 0; i < DerivedTypes.Length; i++)
                {
                    var type = DerivedTypes[i];
                    DerivedTypeNames[i] = ObjectNames.NicifyVariableName(type.Name);
                    DerivedFullTypeNames[i] = type.FullName;
                }
            }

            public Type[] DerivedTypes { get; }

            public string[] DerivedTypeNames { get; }

            public string[] DerivedFullTypeNames { get; }

            private static Assembly GetAssembly(string name)
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(assembly => assembly.GetName().Name == name);
            }
        }
    }
}
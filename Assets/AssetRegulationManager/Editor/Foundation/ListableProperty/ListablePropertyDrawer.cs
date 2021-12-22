// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Foundation.ListableProperty
{
    [CustomPropertyDrawer(typeof(ListableProperty<>), true)]
    internal sealed class ListablePropertyDrawer : PropertyDrawer
    {
        private const string IsListModePropertyName = "_isListMode";
        private const string ValuesPropertyName = "_values";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var fieldRect = position;
            fieldRect.height = EditorGUIUtility.singleLineHeight;

            var isListModeProperty = property.FindPropertyRelative(IsListModePropertyName);
            var valuesProperty = property.FindPropertyRelative(ValuesPropertyName);
            var isListMode = isListModeProperty.boolValue;

            var firstFieldRect = fieldRect;
            firstFieldRect.xMax -= 28;
            var modeButtonRect = fieldRect;
            modeButtonRect.xMin += firstFieldRect.width + EditorGUIUtility.standardVerticalSpacing;

            if (!isListMode)
            {
                if (valuesProperty.arraySize == 0)
                {
                    valuesProperty.arraySize++;
                }

                var firstValueProperty = valuesProperty.GetArrayElementAtIndex(0);
                EditorGUI.PropertyField(firstFieldRect, firstValueProperty, label);

                if (GUI.Button(modeButtonRect, ListablePropertyEditorUtility.ListIcon))
                {
                    isListModeProperty.boolValue = true;
                }
            }
            else
            {
                var labelRect = firstFieldRect;
                labelRect.width = EditorGUIUtility.labelWidth;
                var arraySizeRect = firstFieldRect;
                valuesProperty.isExpanded =
                    EditorGUI.Foldout(labelRect, valuesProperty.isExpanded, $"{label.text} List", true);
                var depth = valuesProperty.depth;
                var isExpanded = valuesProperty.isExpanded;

                valuesProperty.NextVisible(true);
                arraySizeRect.xMin += EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing;
                var indentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
                EditorGUI.PropertyField(arraySizeRect, valuesProperty, new GUIContent());
                EditorGUI.indentLevel = indentLevel;

                if (GUI.Button(modeButtonRect, ListablePropertyEditorUtility.ListIcon))
                {
                    isListModeProperty.boolValue = false;
                }

                EditorGUI.indentLevel++;
                if (isExpanded)
                {
                    while (valuesProperty.NextVisible(false))
                    {
                        if (depth >= valuesProperty.depth)
                        {
                            break;
                        }

                        fieldRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        EditorGUI.PropertyField(fieldRect, valuesProperty);
                    }
                }

                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = 0.0f;
            var isListModeProperty = property.FindPropertyRelative(IsListModePropertyName);
            var valuesProperty = property.FindPropertyRelative(ValuesPropertyName);
            var isListMode = isListModeProperty.boolValue;

            if (!isListMode)
            {
                height += EditorGUIUtility.singleLineHeight;
            }
            else
            {
                var depth = valuesProperty.depth;
                var isExpanded = valuesProperty.isExpanded;
                valuesProperty.NextVisible(true);
                height += EditorGUIUtility.singleLineHeight;
                if (isExpanded)
                {
                    while (valuesProperty.NextVisible(false))
                    {
                        if (depth >= valuesProperty.depth)
                        {
                            break;
                        }

                        height += EditorGUI.GetPropertyHeight(valuesProperty, false);
                        height += EditorGUIUtility.standardVerticalSpacing;
                    }
                }
            }

            return height;
        }
    }
}

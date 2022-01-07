// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Tool.Shared;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorInspectorDrawer
    {
        private readonly SerializedProperty _assetGroupProperty;
        private Vector2 _scrollPosition;

        public AssetRegulationEditorInspectorDrawer(SerializedProperty assetGroupProperty)
        {
            _assetGroupProperty = assetGroupProperty;
        }

        public void OnGUI(Rect rect)
        {
            var filtersProperty = _assetGroupProperty.FindPropertyRelative("_assetGroup._filters");
            var limitationsProperty = _assetGroupProperty.FindPropertyRelative("_assetSpec._limitations");
            var filtersHeight = EditorGUI.GetPropertyHeight(filtersProperty, true);
            var limitationsHeight = EditorGUI.GetPropertyHeight(limitationsProperty, true);

            var scrollViewRect = rect;
            var scrollContentsRect = scrollViewRect;
            scrollContentsRect.height = filtersHeight + limitationsHeight - 20;

            var isScrollBarShown = scrollContentsRect.height >= scrollViewRect.height;
            if (isScrollBarShown)
            {
                scrollContentsRect.xMax -= 14;
            }

            using (var scrollViewScope = new GUI.ScrollViewScope(scrollViewRect, _scrollPosition, scrollContentsRect))
            {
                var fieldRect = scrollContentsRect;
                fieldRect.xMax -= 4;
                fieldRect.xMin += 2;
                fieldRect.height = EditorGUIUtility.singleLineHeight;
                var titleBackgroundRect = rect;
                titleBackgroundRect.height = EditorGUIUtility.singleLineHeight;
                var borderRect = titleBackgroundRect;
                borderRect.height = 1;
                var minusButtonRect = fieldRect;
                minusButtonRect.xMin += fieldRect.width - 20;
                minusButtonRect.width = 20;
                var plusButtonRect = minusButtonRect;
                plusButtonRect.x -= minusButtonRect.width + EditorGUIUtility.standardVerticalSpacing;

                // Draw Asset Group
                EditorGUI.DrawRect(titleBackgroundRect,
                    new Color(62.0f / 255.0f, 62.0f / 255.0f, 62.0f / 255.0f, 1.0f));
                EditorGUI.LabelField(fieldRect, "Asset Group", EditorStyles.boldLabel);
                plusButtonRect.y = fieldRect.y + 1;
                minusButtonRect.y = fieldRect.y + 1;
                if (GUI.Button(plusButtonRect, EditorGUIUtility.IconContent(EditorGUIUtil.ToolbarPlusIconName),
                        GUIStyle.none))
                {
                    filtersProperty.arraySize++;
                }

                GUI.enabled = filtersProperty.arraySize >= 1;
                if (GUI.Button(minusButtonRect, EditorGUIUtility.IconContent(EditorGUIUtil.ToolbarMinusIconName),
                        GUIStyle.none))
                {
                    filtersProperty.arraySize--;
                }

                GUI.enabled = true;


                fieldRect.y += EditorGUIUtility.standardVerticalSpacing;
                fieldRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                for (var i = 0; i < filtersProperty.arraySize; i++)
                {
                    var elementProp = filtersProperty.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(fieldRect, elementProp, true);
                    fieldRect.y += EditorGUI.GetPropertyHeight(elementProp);
                }

                fieldRect.y += EditorGUIUtility.standardVerticalSpacing * 3;

                // Draw Asset Specifications
                borderRect.y = fieldRect.y;
                EditorGUI.DrawRect(borderRect, EditorGUIUtil.EditorBorderColor);
                fieldRect.y += 1;
                titleBackgroundRect.y = fieldRect.y;
                EditorGUI.DrawRect(titleBackgroundRect,
                    new Color(62.0f / 255.0f, 62.0f / 255.0f, 62.0f / 255.0f, 1.0f));
                EditorGUI.LabelField(fieldRect, "Asset Specification", EditorStyles.boldLabel);
                plusButtonRect.y = fieldRect.y + 1;
                minusButtonRect.y = fieldRect.y + 1;
                if (GUI.Button(plusButtonRect, EditorGUIUtility.IconContent(EditorGUIUtil.ToolbarPlusIconName),
                        GUIStyle.none))
                {
                    limitationsProperty.arraySize++;
                }

                GUI.enabled = limitationsProperty.arraySize >= 1;
                if (GUI.Button(minusButtonRect, EditorGUIUtility.IconContent(EditorGUIUtil.ToolbarMinusIconName),
                        GUIStyle.none))
                {
                    limitationsProperty.arraySize--;
                }

                GUI.enabled = true;
                fieldRect.y += EditorGUIUtility.standardVerticalSpacing;
                fieldRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                for (var i = 0; i < limitationsProperty.arraySize; i++)
                {
                    var elementProp = limitationsProperty.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(fieldRect, elementProp, true);
                    fieldRect.y += EditorGUI.GetPropertyHeight(elementProp);
                }

                fieldRect.y += EditorGUIUtility.standardVerticalSpacing * 3;
                borderRect.y = fieldRect.y;
                EditorGUI.DrawRect(borderRect, EditorGUIUtil.EditorBorderColor);

                _scrollPosition = scrollViewScope.scrollPosition;
            }
        }
    }
}

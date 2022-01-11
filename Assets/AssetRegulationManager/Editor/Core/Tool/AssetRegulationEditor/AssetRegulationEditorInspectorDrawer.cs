// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Tool.Shared;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorInspectorDrawer
    {
        private static readonly Color TitleBackgroundColorPro =
            new Color(62.0f / 255.0f, 62.0f / 255.0f, 62.0f / 255.0f, 1.0f);

        private static readonly Color TitleBackgroundColorNotPro =
            new Color(203.0f / 255.0f, 203.0f / 255.0f, 203.0f / 255.0f, 1.0f);

        private readonly SerializedProperty _assetGroupProperty;

        private readonly Dictionary<int, float> _filtersElementHeights = new Dictionary<int, float>();
        private readonly Dictionary<int, float> _limitationsElementHeights = new Dictionary<int, float>();
        private Vector2 _scrollPosition;

        public AssetRegulationEditorInspectorDrawer(SerializedProperty assetGroupProperty)
        {
            _assetGroupProperty = assetGroupProperty;
        }

        private static Color TitleBackgroundColor =>
            EditorGUIUtility.isProSkin ? TitleBackgroundColorPro : TitleBackgroundColorNotPro;

        public void OnGUI(Rect rect)
        {
            var filtersProperty = _assetGroupProperty.FindPropertyRelative("_assetGroup._filters");
            var limitationsProperty = _assetGroupProperty.FindPropertyRelative("_assetSpec._limitations");
            var assetGroupHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 5;
            for (var i = 0; i < filtersProperty.arraySize; i++)
            {
                var elementProp = filtersProperty.GetArrayElementAtIndex(i);
                var elementPropHeight = EditorGUI.GetPropertyHeight(elementProp);
                _filtersElementHeights[i] = elementPropHeight;
                assetGroupHeight += elementPropHeight;
            }

            var assetSpecHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 5;
            for (var i = 0; i < limitationsProperty.arraySize; i++)
            {
                var elementProp = limitationsProperty.GetArrayElementAtIndex(i);
                var elementPropHeight = EditorGUI.GetPropertyHeight(elementProp);
                _limitationsElementHeights[i] = elementPropHeight;
                assetSpecHeight += elementPropHeight;
            }

            var tempRect = rect;
            tempRect.y += assetGroupHeight;
            tempRect.height = assetSpecHeight;

            var scrollViewRect = rect;
            var scrollContentsRect = scrollViewRect;
            scrollContentsRect.height = assetGroupHeight + assetSpecHeight;

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
                EditorGUI.DrawRect(titleBackgroundRect, TitleBackgroundColor);
                EditorGUI.LabelField(fieldRect, "Asset Group", EditorStyles.boldLabel);
                plusButtonRect.y = fieldRect.y + 1;
                minusButtonRect.y = fieldRect.y + 1;
                if (GUI.Button(plusButtonRect, EditorGUIUtility.IconContent(EditorGUIUtil.ToolbarPlusIconName),
                        GUIStyle.none))
                {
                    filtersProperty.arraySize++;
                    var lastIndex = filtersProperty.arraySize - 1;
                    var elementProp = filtersProperty.GetArrayElementAtIndex(lastIndex);
                    var elementPropHeight = EditorGUI.GetPropertyHeight(elementProp, true);
                    _filtersElementHeights[lastIndex] = elementPropHeight;
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
                    fieldRect.y += _filtersElementHeights[i];
                }

                fieldRect.y += EditorGUIUtility.standardVerticalSpacing * 3;

                // Draw Asset Specifications
                borderRect.y = fieldRect.y;
                EditorGUI.DrawRect(borderRect, EditorGUIUtil.EditorBorderColor);
                fieldRect.y += 1;
                titleBackgroundRect.y = fieldRect.y;
                EditorGUI.DrawRect(titleBackgroundRect, TitleBackgroundColor);
                EditorGUI.LabelField(fieldRect, "Asset Specification", EditorStyles.boldLabel);
                plusButtonRect.y = fieldRect.y + 1;
                minusButtonRect.y = fieldRect.y + 1;
                if (GUI.Button(plusButtonRect, EditorGUIUtility.IconContent(EditorGUIUtil.ToolbarPlusIconName),
                        GUIStyle.none))
                {
                    limitationsProperty.arraySize++;
                    var lastIndex = limitationsProperty.arraySize - 1;
                    var elementProp = limitationsProperty.GetArrayElementAtIndex(lastIndex);
                    var elementPropHeight = EditorGUI.GetPropertyHeight(elementProp, true);
                    _limitationsElementHeights[lastIndex] = elementPropHeight;
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
                    fieldRect.y += _limitationsElementHeights[i];
                }

                fieldRect.y += EditorGUIUtility.standardVerticalSpacing * 3;
                borderRect.y = fieldRect.y;
                EditorGUI.DrawRect(borderRect, EditorGUIUtil.EditorBorderColor);

                _scrollPosition = scrollViewScope.scrollPosition;
            }
        }
    }
}

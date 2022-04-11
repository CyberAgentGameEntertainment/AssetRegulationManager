// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Shared
{
    internal static class EditorGUIUtil
    {
        private static readonly Color BorderColorPro =
            new Color(25.0f / 255.0f, 25.0f / 255.0f, 25.0f / 255.0f, 1.0f);

        private static readonly Color BorderColorNotPro =
            new Color(138.0f / 255.0f, 138.0f / 255.0f, 138.0f / 255.0f, 1.0f);

        private static readonly Color TitleBackgroundColorPro =
            new Color(62.0f / 255.0f, 62.0f / 255.0f, 62.0f / 255.0f, 1.0f);

        private static readonly Color TitleBackgroundColorNotPro =
            new Color(203.0f / 255.0f, 203.0f / 255.0f, 203.0f / 255.0f, 1.0f);
        
        public static Color EditorBorderColor =>
            EditorGUIUtility.isProSkin ? BorderColorPro : BorderColorNotPro;

        public static Color TitleBackgroundColor =>
            EditorGUIUtility.isProSkin ? TitleBackgroundColorPro : TitleBackgroundColorNotPro;

        public const string ToolbarPlusIconName = "Toolbar Plus";
        public const string ToolbarMinusIconName = "Toolbar Minus";
    }
}

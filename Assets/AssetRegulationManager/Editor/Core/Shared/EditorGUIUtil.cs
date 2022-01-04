// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.Shared
{
    internal static class EditorGUIUtil
    {
        private static readonly Color EditorBorderColorPro =
            new Color(25.0f / 255.0f, 25.0f / 255.0f, 25.0f / 255.0f, 1.0f);

        private static readonly Color EditorBorderColorNotPro =
            new Color(138.0f / 255.0f, 138.0f / 255.0f, 138.0f / 255.0f, 1.0f);

        public static Color EditorBorderColor =>
            EditorGUIUtility.isProSkin ? EditorBorderColorPro : EditorBorderColorNotPro;

        public const string ToolbarPlusIconName = "Toolbar Plus";
        public const string ToolbarMinusIconName = "Toolbar Minus";
    }
}

// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model
{
    public sealed class AssetSelectionService
    {
        public void Run(string assetPath)
        {
            var o = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            Selection.activeObject = o;
        }
    }
}

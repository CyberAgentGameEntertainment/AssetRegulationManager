// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model
{
    public sealed class AssetSelection
    {
        public void Run(string assetPath)
        {
            var selectionObj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            Selection.activeObject = selectionObj;
        }
    }
}

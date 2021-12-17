// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.Adapters
{
    public class AssetDatabaseAdapter : IAssetDatabaseAdapter
    {
        public IEnumerable<string> FindAssetPaths(string filter)
        {
            return AssetDatabase.FindAssets(filter).Select(AssetDatabase.GUIDToAssetPath);
        }

        public TAsset LoadAssetAtPath<TAsset>(string assetPath) where TAsset : Object
        {
            return AssetDatabase.LoadAssetAtPath<TAsset>(assetPath);
        }
    }
}
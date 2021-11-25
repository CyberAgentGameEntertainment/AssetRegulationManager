// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using UnityEditor;
using UnityEngine;

namespace Develop.Editor
{
    public class CreateAssetRegulationCollection
    {
        [MenuItem("Develop/CreateAssetRegulationCollection")]
        private static void CreateScriptableObject()
        {
            var assetRegulationCollection = ScriptableObject.CreateInstance<AssetRegulationCollection>();
            assetRegulationCollection.Regulations.Add(new AssetRegulation("rule1",
                "Assets/Develop/Textures",
                new List<IAssetRegulationEntry> {new TextureSizeRegulationEntry(new Vector2(128, 128))}));
            var fileName = "AssetRegulationCollection.asset";
            var path = "Assets/Develop";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            AssetDatabase.CreateAsset(assetRegulationCollection, Path.Combine(path, fileName));
        }
    }
}
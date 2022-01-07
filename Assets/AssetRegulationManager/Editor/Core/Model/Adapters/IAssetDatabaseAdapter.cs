// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.Adapters
{
    /// <summary>
    ///     Interface to abstract <see cref="AssetDatabase" />.
    /// </summary>
    public interface IAssetDatabaseAdapter
    {
        IEnumerable<string> FindAssetPaths(string filter);

        TAsset LoadAssetAtPath<TAsset>(string assetPath) where TAsset : Object;
    }
}

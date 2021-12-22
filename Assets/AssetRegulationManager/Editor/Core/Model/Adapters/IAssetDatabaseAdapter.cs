// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.Adapters
{
    public interface IAssetDatabaseAdapter
    {
        IEnumerable<string> FindAssetPaths(string filter);

        TAsset LoadAssetAtPath<TAsset>(string assetPath) where TAsset : Object;
    }
}

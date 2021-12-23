// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Data
{
    public sealed class AssetRegulationRepository
    {
        /// <summary>
        ///     Get all <see cref="AssetRegulation" /> defined in the project.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AssetRegulation> GetAllRegulations()
        {
            return AssetDatabase
                .FindAssets($"t:{nameof(AssetRegulationSettings)}")
                .SelectMany(x =>
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(x);
                    var settings = AssetDatabase.LoadAssetAtPath<AssetRegulationSettings>(assetPath);
                    return settings.Regulations;
                });
        }
    }
}

// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    public interface IAssetFilter
    {
        string Id { get; }
        
        void SetupForMatching();

        /// <summary>
        ///     Return true if the asset passes this filter.
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="assetType"></param>
        /// <returns></returns>
        bool IsMatch(string assetPath, Type assetType);

        string GetDescription();

        void OverwriteValuesFromJson(string json);
    }
}

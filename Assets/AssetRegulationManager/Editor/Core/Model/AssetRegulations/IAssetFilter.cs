// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    public interface IAssetFilter
    {
        void Setup();

        /// <summary>
        ///     Return true if the <see cref="assetPath" /> asset passes this filter.
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        bool IsMatch(string assetPath);

        string GetDescription();
    }
}

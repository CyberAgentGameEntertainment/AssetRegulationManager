// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    /// <summary>
    ///     Class to test if an asset is within the limitation.
    /// </summary>
    public interface IAssetLimitation
    {
        /// <summary>
        ///     Test if <see cref="obj" /> is within the limitation.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Check(Object obj);

        string GetDescription();

        string GetLatestValueAsText();
    }
}

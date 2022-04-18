// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    /// <summary>
    ///     Class to test if an asset is within the constraint.
    /// </summary>
    public interface IAssetConstraint
    {
        string Id { get; }
        
        /// <summary>
        ///     Test if <see cref="obj" /> is within the constraint.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Check(Object obj);

        string GetDescription();

        string GetLatestValueAsText();
    }
}

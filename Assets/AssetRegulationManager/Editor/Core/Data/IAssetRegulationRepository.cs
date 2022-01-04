// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;

namespace AssetRegulationManager.Editor.Core.Data
{
    public interface IAssetRegulationRepository
    {
        /// <summary>
        ///     Get all <see cref="AssetRegulation" /> defined in the project.
        /// </summary>
        /// <returns></returns>
        IEnumerable<AssetRegulation> GetAllRegulations();
    }
}

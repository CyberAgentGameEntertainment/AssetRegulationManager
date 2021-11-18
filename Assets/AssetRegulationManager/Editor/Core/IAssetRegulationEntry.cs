// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEngine;

namespace AssetRegulationManager.Editor.Core
{
    /// <summary>
    ///     The base class of the Asset Regulation Entry.
    /// </summary>
    public interface IAssetRegulationEntry
    {
        /// <summary>
        ///     Label for GUI.
        /// </summary>
        string Label { get; }

        /// <summary>
        ///     Regulation explanation.
        /// </summary>
        string Explanation { get; }

        /// <summary>
        ///     call AssetRegulationEntry<TAsset>.RunTest(TAsset asset) by assigning obj.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool RunTest(Object obj);

        /// <summary>
        ///     Draw the regulations in the GUI.
        /// </summary>
        void DrawGUI();
    }
}
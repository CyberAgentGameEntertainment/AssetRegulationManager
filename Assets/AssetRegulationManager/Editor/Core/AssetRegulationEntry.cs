// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using NUnit.Framework;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core
{
    /// <summary>
    ///     The base generic class of the Asset Regulation Entry.
    /// </summary>
    public abstract class AssetRegulationEntry<TAsset> : IAssetRegulationEntry where TAsset : Object
    {
        public abstract string Label { get; }
        public abstract string Explanation { get; }
        public abstract void DrawGUI();
        
        bool IAssetRegulationEntry.RunTest(Object obj)
        {
            Assert.IsTrue(obj is TAsset);

            return RunTest((TAsset) obj);
        }
        
        /// <summary>
        ///    Determine if you are following the regulations.
        /// <param name="asset"></param>
        /// </summary>
        protected abstract bool RunTest(TAsset asset);
    }
}
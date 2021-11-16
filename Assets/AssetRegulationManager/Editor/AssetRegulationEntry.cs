// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using NUnit.Framework;
using UnityEngine;

namespace AssetRegulationManager.Editor
{
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

        protected abstract bool RunTest(TAsset asset);
    }
}
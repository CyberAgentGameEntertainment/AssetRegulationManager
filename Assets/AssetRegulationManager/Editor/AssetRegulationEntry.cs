// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using NUnit.Framework;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor
{
    public abstract class AssetRegulationEntry<TAsset> : IAssetRegulationEntry where TAsset : Object
    {
        public abstract string Label { get; }
        public abstract string Explanation { get; }
        public abstract void DrawGUI();
        protected abstract bool RunTest(TAsset asset);
        
        bool IAssetRegulationEntry.RunTest(Object obj)
        {
            Assert.IsTrue(obj is TAsset);

            return RunTest((TAsset) obj);
        }
    }
}
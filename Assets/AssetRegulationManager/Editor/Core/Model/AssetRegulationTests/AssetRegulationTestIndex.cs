// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTests
{
    public class AssetRegulationTestIndex
    {
        public AssetRegulationTestIndex(int testIndex, int testEntryIndex)
        {
            TestIndex = testIndex;
            TestEntryIndex = testEntryIndex;
        }

        public int TestIndex { get; }
        public int TestEntryIndex { get; }
    }
}
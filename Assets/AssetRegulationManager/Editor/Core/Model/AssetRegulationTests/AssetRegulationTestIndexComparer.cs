// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTests
{
    public class AssetRegulationTestIndexComparer : IEqualityComparer<AssetRegulationTestIndex>
    {
        public bool Equals(AssetRegulationTestIndex x, AssetRegulationTestIndex y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.TestIndex == y.TestIndex && x.TestEntryIndex == y.TestEntryIndex;
        }

        public int GetHashCode(AssetRegulationTestIndex obj)
        {
            unchecked
            {
                return (obj.TestIndex * 397) ^ obj.TestEntryIndex;
            }
        }
    }
}
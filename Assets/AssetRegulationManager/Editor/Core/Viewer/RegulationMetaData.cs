// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class RegulationMetaDatum : IComparable<RegulationMetaDatum>
    {
        internal RegulationMetaDatum(string regulationId, int entryIndex)
        {
            RegulationId = regulationId;
            EntryIndex = entryIndex;
        }

        internal string RegulationId { get; }
        internal int EntryIndex { get; }

        public int CompareTo(RegulationMetaDatum other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var regulationIdComparison = string.Compare(RegulationId, other.RegulationId, StringComparison.Ordinal);
            if (regulationIdComparison != 0) return regulationIdComparison;
            return EntryIndex.CompareTo(other.EntryIndex);
        }

        public override int GetHashCode()
        {
            return (RegulationId, EntryIndex).GetHashCode();
        }
    }
}
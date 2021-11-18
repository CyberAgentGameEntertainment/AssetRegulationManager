// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class RegulationEntryViewDatum
    {
        internal RegulationEntryViewDatum(string id, int index, string explanation, TestResultType resultType)
        {
            Id = id;
            Index = index;
            Explanation = explanation;
            ResultType = resultType;
        }

        internal string Id { get; }
        internal int Index { get; }
        internal string Explanation { get; }
        internal TestResultType ResultType { get; }
    }
}
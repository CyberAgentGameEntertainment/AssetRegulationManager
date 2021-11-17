// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AssetRegulationManager.Editor.Core.Viewer
{
    public class RegulationEntryViewDatum
    {
        public string Id { get; }
        public int Index { get; }
        public string Explanation { get; }
        public TestResultType ResultType { get; }

        public RegulationEntryViewDatum(string id, int index, string explanation, TestResultType resultType)
        {
            Id = id;
            Index = index;
            Explanation = explanation;
            ResultType = resultType;
        }
    }
}
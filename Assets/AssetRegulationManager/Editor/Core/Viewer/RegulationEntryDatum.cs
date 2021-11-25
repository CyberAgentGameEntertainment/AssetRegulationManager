// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Foundation.Observable.ObservableProperty;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class RegulationEntryDatum
    {
        internal RegulationEntryDatum(RegulationMetaDatum metaDatum, string path, string explanation)
        {
            MetaDatum = metaDatum;
            Path = path;
            Explanation = explanation;
        }

        internal RegulationMetaDatum MetaDatum { get; }
        internal string Path { get; }
        internal string Explanation { get; }

        internal ObservableProperty<TestResultType> ResultType { get; } =
            new ObservableProperty<TestResultType>(TestResultType.None);
    }
}
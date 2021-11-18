// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class RegulationViewDatum
    {
        internal RegulationViewDatum(string path, List<RegulationEntryViewDatum> entryViewData)
        {
            Path = path;
            EntryViewData = entryViewData.AsReadOnly();
        }

        internal string Path { get; }
        internal IReadOnlyCollection<RegulationEntryViewDatum> EntryViewData { get; }
    }
}
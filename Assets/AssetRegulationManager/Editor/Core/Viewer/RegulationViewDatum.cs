using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Viewer
{

    internal class RegulationViewDatum
    {
        internal string Path { get; }
        internal IReadOnlyCollection<RegulationEntryViewDatum> EntryViewData { get; }

        internal RegulationViewDatum(string path, List<RegulationEntryViewDatum> entryViewData)
        {
            Path = path;
            EntryViewData = entryViewData.AsReadOnly();
        }
    }
}

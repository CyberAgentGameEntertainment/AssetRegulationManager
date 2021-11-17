using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Viewer
{

    public class RegulationViewDatum
    {
        public string Path { get; }
        public IReadOnlyCollection<RegulationEntryViewDatum> EntryViewData { get; }

        public RegulationViewDatum(string path, List<RegulationEntryViewDatum> entryViewData)
        {
            Path = path;
            EntryViewData = entryViewData.AsReadOnly();
        }
    }
}

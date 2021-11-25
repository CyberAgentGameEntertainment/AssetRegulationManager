// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class TestCollection
    {
        private readonly TestJob _job;

        internal TestCollection(TestJob job, List<RegulationEntryDatum> entryData)
        {
            _job = job;
            EntryData = entryData;
        }

        internal IReadOnlyList<RegulationEntryDatum> EntryData { get; }

        internal void Run(IEnumerable<RegulationMetaDatum> metadata)
        {
            foreach (var metaDatum in metadata)
            {
                var entryDatum = EntryData.FirstOrDefault(x => x.MetaDatum.Equals(metaDatum));

                if (entryDatum == null)
                    continue;

                entryDatum.ResultType.Value = _job.Run(metaDatum, entryDatum.Path);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Foundation.Observable;
using AssetRegulationManager.Editor.Foundation.Observable.ObservableProperty;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal class TestCollection
    {
        private TestJob _job;
        internal IReadOnlyList<RegulationEntryDatum> EntryData { get; }
        
        internal TestCollection(TestJob job, List<RegulationEntryDatum> entryData)
        {
            _job = job;
            EntryData = entryData;
        }

        internal void Run(IEnumerable<RegulationMetaDatum> metadata)
        {
            foreach (var metaDatum in metadata)
            {
                var entryDatum = EntryData.FirstOrDefault(x => x.MetaDatum.Equals(metaDatum));
                
                if(entryDatum == null)
                    continue;
                
                entryDatum.ResultType.Value = _job.Run(metaDatum, entryDatum.Path);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    public class RegulationRegexFormatter
    {
        private readonly RegulationViewerStore _store;

        internal RegulationRegexFormatter(RegulationViewerStore store)
        {
            _store = store;
        }
        
        internal List<RegulationEntryDatum> CreateRegulationViewData(string assetPathOrFilter)
        {
            return CreateRegulationViewData(AssetDatabase.FindAssets(assetPathOrFilter)
                .Select(AssetDatabase.GUIDToAssetPath));
        }

        internal List<RegulationEntryDatum> CreateRegulationViewData(IEnumerable<string> paths)
        {
            return paths.Select(CreateRegulationViewDatum).SelectMany(x => x).ToList();
        }

        /// <summary>
        ///     Create a regulation view data from path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<RegulationEntryDatum> CreateRegulationViewDatum(string path)
        {
            var entryViewData = new List<RegulationEntryDatum>();

            // Loop through regulations matched by regex
            foreach (var regulation in _store.AssetRegulationCollection.Regulations.Where(x => Regex.IsMatch(path, x.AssetPathRegex)))
                // Loop with index
            foreach (var entryItem in regulation.Entries.Select((value, index) => new {value, index}))
                entryViewData.Add(new RegulationEntryDatum(new RegulationMetaDatum(regulation.Id, entryItem.index), path, entryItem.value.Explanation));

            return entryViewData;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AssetRegulationManager.Editor.Core;
using AssetRegulationManager.Editor.Foundation.Observable;
using UnityEditor;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    public class RegulationViewerModel
    {
        internal IObservable<IEnumerable<RegulationViewDatum>> FormatViewDataObservable => _formatViewDataSubject;
        internal IObservable<IEnumerable<RegulationEntryViewDatum>> TestResultObservable => _testResultSubject;
        
        private readonly Subject<IEnumerable<RegulationViewDatum>> _formatViewDataSubject = new Subject<IEnumerable<RegulationViewDatum>>();
        private readonly Subject<IEnumerable<RegulationEntryViewDatum>> _testResultSubject = new Subject<IEnumerable<RegulationEntryViewDatum>>();
        private List<AssetRegulation> _regulations;

        internal RegulationViewerModel(List<AssetRegulation> regulations)
        {
            _regulations = regulations;
        }
        
        internal void SearchAssets(string searchText)
        {
            var viewData = AssetDatabase.FindAssets(searchText)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(CreateRegulationViewDatum);
            
            _formatViewDataSubject.OnNext(viewData);
        }
        
        /// <summary>
        ///     
        /// </summary>
        /// <param name="viewData"></param>
        /// <returns></returns>
        internal void RunTest(IEnumerable<RegulationViewDatum> viewData)
        {
            var entryViewData = new List<RegulationEntryViewDatum>();

            foreach (var viewDatum in viewData)
            {
                // Loop grouped by id
                foreach (var viewDataGroup in viewDatum.EntryViewData.GroupBy(x => x.Id))
                {
                    var regulation = _regulations.FirstOrDefault(x => x.Id == viewDataGroup.Key);
                
                    foreach (var entryViewDatum in viewDataGroup)
                    {
                        var entry = regulation?.Entries[entryViewDatum.Index];
                        var obj = AssetDatabase.LoadAssetAtPath<Object>(viewDatum.Path);
                        
                        var testResult = entry?.RunTest(obj) ?? false ? TestResultType.Success : TestResultType.Failed;

                        entryViewData.Add(new RegulationEntryViewDatum(entryViewDatum.Id, entryViewDatum.Index, entry?.Explanation, testResult));
                    }
                }
            }
            
            _testResultSubject.OnNext(entryViewData);
        }
        
        /// <summary>
        ///     Create a regulation view data from path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private RegulationViewDatum CreateRegulationViewDatum(string path)
        {
            var entryViewData = new List<RegulationEntryViewDatum>();
            
            // Loop through regulations matched by regex
            foreach (var regulation in _regulations.Where(x => Regex.IsMatch(path, x.AssetPathRegex)))
            {
                // Loop with index
                foreach (var entryItem in regulation.Entries.Select((value, index) => new { value, index }))
                {
                    entryViewData.Add(new RegulationEntryViewDatum(regulation.Id, entryItem.index, entryItem.value.Explanation, TestResultType.None));
                }
            }

            return new RegulationViewDatum(path, entryViewData);
        }
    }
}

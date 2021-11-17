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
        public IObservable<IEnumerable<RegulationViewDatum>> FormatViewDataObservable => _formatViewDataSubject;
        public IObservable<IEnumerable<RegulationEntryViewDatum>> TestResultObservable => _testResultSubject;
        
        private readonly Subject<IEnumerable<RegulationViewDatum>> _formatViewDataSubject = new Subject<IEnumerable<RegulationViewDatum>>();
        private readonly Subject<IEnumerable<RegulationEntryViewDatum>> _testResultSubject = new Subject<IEnumerable<RegulationEntryViewDatum>>();
        private List<AssetRegulation> _regulations;

        public RegulationViewerModel(List<AssetRegulation> regulations)
        {
            _regulations = regulations;
        }
        
        public void SearchAssets(string searchText)
        {
            var viewData = AssetDatabase.FindAssets(searchText)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(CreateRegulationViewDatum);
            
            _formatViewDataSubject.OnNext(viewData);
        }

        public void RunTest(IEnumerable<RegulationViewDatum> viewData)
        {
            var entryViewData = new List<RegulationEntryViewDatum>();

            foreach (var viewDatum in viewData)
            {
                foreach (var metaGroup in viewDatum.EntryViewData.Select(x => new {x.Id, x.Index}).GroupBy(x => x.Id))
                {
                    var regulation = _regulations.FirstOrDefault(x => x.Id == metaGroup.Key);
                
                    foreach (var meta in metaGroup)
                    {
                        var entry = regulation?.Entries[meta.Index];
                    
                        var obj = AssetDatabase.LoadAssetAtPath<Object>(viewDatum.Path);

                        entryViewData.Add(new RegulationEntryViewDatum(meta.Id, meta.Index, entry?.Explanation, entry?.RunTest(obj) ?? false ? TestResultType.Success : TestResultType.Failed));
                    }
                }
            }
            
            _testResultSubject.OnNext(entryViewData);
        }

        private RegulationViewDatum CreateRegulationViewDatum(string path)
        {
            var entryViewData = new List<RegulationEntryViewDatum>();

            foreach (var regulation in _regulations.Where(x => Regex.IsMatch(path, x.AssetPathRegex)))
            {
                foreach (var entryItem in regulation.Entries.Select((value, index) => new { value, index }))
                {
                    entryViewData.Add(new RegulationEntryViewDatum(regulation.Id, entryItem.index, entryItem.value.Explanation, TestResultType.None));
                }
            }

            return new RegulationViewDatum(path, entryViewData);
        }
    }
}

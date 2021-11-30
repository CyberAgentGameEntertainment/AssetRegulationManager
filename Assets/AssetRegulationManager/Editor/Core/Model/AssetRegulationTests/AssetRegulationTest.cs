// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.Observable.ObservableCollection;
using AssetRegulationManager.Editor.Foundation.Observable.ObservableProperty;
using UnityEditor;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTests
{
    public class AssetRegulationTest
    {
        private readonly ObservableList<AssetRegulationTestEntry> _entries =
            new ObservableList<AssetRegulationTestEntry>();

        public AssetRegulationTest(string assetPath, IEnumerable<IAssetRegulationEntry> regulationEntries)
        {
            AssetPath = assetPath;
            foreach (var regulationEntry in regulationEntries)
                _entries.Add(new AssetRegulationTestEntry(regulationEntry,Guid.NewGuid().ToString()));
        }

        internal IReadOnlyObservableList<AssetRegulationTestEntry> Entries => _entries;
        
        internal ObservableProperty<AssetRegulationTestResultType> Status { get; } =
            new ObservableProperty<AssetRegulationTestResultType>(AssetRegulationTestResultType.None);

        internal string AssetPath { get; }

        internal void RunAll()
        {
            var _ = RunSelectionTest(Entries);
        }

        internal void RunSelection(IReadOnlyCollection<string> selectionEntryIds)
        {
            var selectionEntries = Entries.Where(x => selectionEntryIds.Contains(x.Id)).ToList().AsReadOnly();
            
            if(!selectionEntries.Any())
                return;

            var _ = RunSelectionTest(selectionEntries);
        }

        private async Task RunSelectionTest(IReadOnlyCollection<AssetRegulationTestEntry> selectionEntries)
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>(AssetPath);

            foreach (var entry in selectionEntries)
            {
                entry.Run(asset);
                await Task.Delay(1);
            }
            
            if (selectionEntries.All(x => x.Status.Value == AssetRegulationTestResultType.Success))
                Status.Value = AssetRegulationTestResultType.Success;
            if (selectionEntries.Any(x => x.Status.Value == AssetRegulationTestResultType.Failed))
                Status.Value = AssetRegulationTestResultType.Failed;
        }
    }
}
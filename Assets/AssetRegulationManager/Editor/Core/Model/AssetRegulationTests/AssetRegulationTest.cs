// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.Observable.ObservableCollection;
using UnityEditor;
using UnityEngine;

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
                _entries.Add(new AssetRegulationTestEntry(regulationEntry));
        }

        internal IReadOnlyObservableList<AssetRegulationTestEntry> Entries => _entries;

        internal string AssetPath { get; }

        internal void RunAll()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>(AssetPath);
            foreach (var entry in Entries) entry.Run(asset);
        }

        internal void RunSelection(IEnumerable<string> selectionEntryIds)
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>(AssetPath);

            foreach (var entry in Entries.Where(x => selectionEntryIds.Contains(x.Id)))
                entry.Run(asset);
        }
    }
}
// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
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

        public IReadOnlyObservableList<AssetRegulationTestEntry> Entries => _entries;

        public string AssetPath { get; }

        public void RunAll()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>(AssetPath);
            foreach (var entry in Entries) entry.Run(asset);
        }

        public void RunSelection(IEnumerable<int> selectIndex)
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>(AssetPath);
            foreach (var index in selectIndex) Entries[index].Run(asset);
        }
    }
}
// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model.Adapters;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.Observable.ObservableCollection;
using AssetRegulationManager.Editor.Foundation.Observable.ObservableProperty;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTests
{
    public sealed class AssetRegulationTest
    {
        private readonly IAssetDatabaseAdapter _assetDatabaseAdapter;

        private readonly ObservableDictionary<string, AssetRegulationTestEntry> _entries =
            new ObservableDictionary<string, AssetRegulationTestEntry>();

        public AssetRegulationTest(string assetPath, IAssetDatabaseAdapter assetDatabaseAdapter)
        {
            Id = Guid.NewGuid().ToString();
            AssetPath = assetPath;
            _assetDatabaseAdapter = assetDatabaseAdapter;
        }

        public string Id { get; }

        public IReadOnlyObservableDictionary<string, AssetRegulationTestEntry> Entries => _entries;

        /// <summary>
        ///     Latest execution status.
        ///     <list type="bullet">
        ///         <item><see cref="AssetRegulationTestStatus.Success" />: All of the executed entries are successful</item>
        ///         <item><see cref="AssetRegulationTestStatus.Failed" />: Any of the executed entries fails</item>
        ///     </list>
        /// </summary>
        public ObservableProperty<AssetRegulationTestStatus> LatestStatus { get; } =
            new ObservableProperty<AssetRegulationTestStatus>(AssetRegulationTestStatus.None);

        public string AssetPath { get; }

        public string AddEntry(IAssetRegulationEntry regulationEntry)
        {
            var entry = new AssetRegulationTestEntry(regulationEntry);
            _entries.Add(entry.Id, entry);
            return entry.Id;
        }

        public void RemoveEntry(string id)
        {
            _entries.Remove(id);
        }

        public void ClearEntries()
        {
            _entries.Clear();
        }

        public IEnumerable CreateRunAllSequence()
        {
            var entryIds = _entries.Values.Select(x => x.Id).ToArray();
            return CreateRunSequence(entryIds);
        }

        public IEnumerable CreateRunSequence(IReadOnlyList<string> entryIds)
        {
            var status = AssetRegulationTestStatus.Success;
            var asset = _assetDatabaseAdapter.LoadAssetAtPath<Object>(AssetPath);
            foreach (var entry in _entries.Values)
            {
                if (!entryIds.Contains(entry.Id))
                {
                    continue;
                }

                entry.Run(asset);
                if (entry.Status.Value == AssetRegulationTestStatus.Failed)
                {
                    status = AssetRegulationTestStatus.Failed;
                }

                yield return null;
            }

            LatestStatus.Value = status;
        }
    }
}
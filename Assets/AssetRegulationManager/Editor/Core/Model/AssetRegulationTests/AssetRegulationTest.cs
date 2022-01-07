// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
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

        internal string AddLimitation(IAssetLimitation limitation)
        {
            var entry = new AssetRegulationTestEntry(limitation);
            _entries.Add(entry.Id, entry);
            return entry.Id;
        }

        internal void RemoveLimitation(string id)
        {
            _entries.Remove(id);
        }

        internal void ClearLimitations()
        {
            _entries.Clear();
        }

        internal void ClearAllStatus()
        {
            foreach (var entry in _entries.Values)
            {
                entry.ClearStatus();
            }

            LatestStatus.Value = AssetRegulationTestStatus.None;
        }

        internal void ClearStatus(IReadOnlyList<string> entryIds)
        {
            foreach (var entry in _entries.Values)
            {
                entry.ClearStatus();
            }

            LatestStatus.Value = AssetRegulationTestStatus.None;
        }

        internal IEnumerable CreateRunAllSequence()
        {
            var entryIds = _entries.Values.Select(x => x.Id).ToArray();
            return CreateRunSequence(entryIds);
        }

        internal IEnumerable CreateRunSequence(IReadOnlyList<string> entryIds)
        {
            var status = AssetRegulationTestStatus.Success;
            var asset = _assetDatabaseAdapter.LoadAssetAtPath<Object>(AssetPath);
            foreach (var entry in _entries.Values)
            {
                if (!entryIds.Contains(entry.Id))
                {
                    continue;
                }

                yield return null;

                entry.Run(asset);
                if (entry.Status.Value == AssetRegulationTestStatus.Failed)
                {
                    status = AssetRegulationTestStatus.Failed;
                }
            }

            LatestStatus.Value = status;
        }
    }
}

// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model.Adapters;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;
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

        private readonly ObservableProperty<AssetRegulationTestStatus> _latestStatus =
            new ObservableProperty<AssetRegulationTestStatus>(AssetRegulationTestStatus.None);

        /// <summary>
        ///     Latest execution status.
        ///     <list type="bullet">
        ///         <item><see cref="AssetRegulationTestStatus.Success" />: All of the executed entries are successful</item>
        ///         <item><see cref="AssetRegulationTestStatus.Failed" />: Any of the executed entries fails</item>
        ///     </list>
        /// </summary>
        public IReadOnlyObservableProperty<AssetRegulationTestStatus> LatestStatus => _latestStatus;

        public string AssetPath { get; }

        internal string AddEntry(IAssetLimitation limitation)
        {
            var entry = new AssetRegulationTestEntry(limitation);
            _entries.Add(entry.Id, entry);
            return entry.Id;
        }

        internal void RemoveEntry(string id)
        {
            _entries.Remove(id);
        }

        internal void ClearEntries()
        {
            _entries.Clear();
        }

        internal void ClearAllStatus()
        {
            foreach (var entry in _entries.Values)
            {
                entry.ClearStatus();
            }

            _latestStatus.Value = AssetRegulationTestStatus.None;
        }

        internal void ClearStatus(IReadOnlyList<string> entryIds)
        {
            foreach (var entry in _entries.Values)
            {
                entry.ClearStatus();
            }

            _latestStatus.Value = AssetRegulationTestStatus.None;
        }

        internal void RunAll()
        {
            var entryIds = _entries.Values.Select(x => x.Id).ToArray();
            Run(entryIds);
        }

        internal void Run(IReadOnlyList<string> entryIds)
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
            }

            _latestStatus.Value = status;
        }
    }
}

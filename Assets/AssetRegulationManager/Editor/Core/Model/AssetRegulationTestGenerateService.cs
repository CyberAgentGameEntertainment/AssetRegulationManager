// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.Adapters;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model
{
    public sealed class AssetRegulationTestGenerateService
    {
        private readonly AssetRegulationManagerStore _store;
        private readonly IAssetDatabaseAdapter _assetDatabaseAdapter;

        public AssetRegulationTestGenerateService(AssetRegulationManagerStore store,
            IAssetDatabaseAdapter assetDatabaseAdapter)
        {
            _store = store;
            _assetDatabaseAdapter = assetDatabaseAdapter;
        }

        public void Run(string assetPathOrFilter)
        {
            var assetPaths = _assetDatabaseAdapter.FindAssetPaths(assetPathOrFilter).ToArray();

            _store.ClearTests();

            var tests = new Dictionary<string, AssetRegulationTest>();
            foreach (var regulation in _store.Regulations)
            {
                var regex = new Regex(regulation.AssetPathRegex);
                foreach (var assetPath in assetPaths)
                {
                    if (!regex.IsMatch(assetPath))
                    {
                        continue;
                    }

                    if (!tests.TryGetValue(assetPath, out var test))
                    {
                        test = new AssetRegulationTest(assetPath, _assetDatabaseAdapter);
                        tests.Add(assetPath, test);
                    }

                    foreach (var entry in regulation.Entries)
                    {
                        test.AddEntry(entry);
                    }
                }
            }

            _store.AddTests(tests.Values);
        }
    }
}
// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulationTests
{
    public sealed class AssetRegulationTestGenerateService
    {
        private readonly RegulationManagerStore _store;

        public AssetRegulationTestGenerateService(RegulationManagerStore store)
        {
            _store = store;
        }

        public void Run(string assetPathOrFilter)
        {
            var assetPaths = AssetDatabase.FindAssets(assetPathOrFilter).Select(AssetDatabase.GUIDToAssetPath)
                .ToArray();

            _store.Tests.Clear();

            foreach (var regulation in _store.Regulations)
            {
                var regex = new Regex(regulation.AssetPathRegex);
                foreach (var assetPath in assetPaths)
                    if (regex.IsMatch(assetPath))
                        _store.Tests.Add(new AssetRegulationTest(assetPath, regulation.Entries));
            }
        }
    }
}
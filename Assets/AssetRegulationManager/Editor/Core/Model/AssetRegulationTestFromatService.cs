// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;

namespace AssetRegulationManager.Editor.Core.Model
{
    public class AssetRegulationTestFormatService
    {
        private readonly IAssetRegulationTestStore _store;

        public AssetRegulationTestFormatService(IAssetRegulationTestStore store)
        {
            _store = store;
        }
        
        public IReadOnlyCollection<AssetRegulationTest> Run(bool excludeEmptyTests)
        {
            return _store.Tests.Values.Where(test => !excludeEmptyTests || test.Entries.Any()).ToList();
        }
    }
}

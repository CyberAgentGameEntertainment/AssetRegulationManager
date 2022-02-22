// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Data;

namespace AssetRegulationManager.Editor.Core.Model
{
    public sealed class AssetRegulationTestExecuteService
    {
        private readonly IAssetRegulationTestStore _store;
        private readonly AssetRegulationTestFormatService _formatService;

        public AssetRegulationTestExecuteService(IAssetRegulationTestStore store, AssetRegulationTestFormatService formatService)
        {
            _store = store;
            _formatService = formatService;
        }

        public void ClearAllResults(bool excludeEmptyTests)
        {
            foreach (var test in _formatService.Run(excludeEmptyTests))
            {
                ClearResults(test.Id);
            }
        }

        public void ClearResults(string testId)
        {
            var test = _store.Tests[testId];
            test.ClearAllStatus();
        }

        public void ClearResults(string testId, IReadOnlyList<string> entryIds)
        {
            var test = _store.Tests[testId];
            test.ClearStatus(entryIds);
        }

        public void RunAll(bool excludeEmptyTests)
        {
            foreach (var test in _formatService.Run(excludeEmptyTests))
            {
                test.RunAll();
            }
        }

        public void Run(string testId)
        {
            var test = _store.Tests[testId];
            test.RunAll();
        }

        public void Run(string testId, IReadOnlyList<string> entryIds)
        {
            var test = _store.Tests[testId];
            test.Run(entryIds);
        }
    }
}

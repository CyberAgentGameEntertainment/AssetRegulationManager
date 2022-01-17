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

        public AssetRegulationTestExecuteService(IAssetRegulationTestStore store)
        {
            _store = store;
        }

        public void ClearAllResults()
        {
            foreach (var test in _store.Tests.Values)
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

        public void RunAll()
        {
            foreach (var test in _store.Tests.Values)
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

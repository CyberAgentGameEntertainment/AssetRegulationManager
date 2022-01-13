// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections;
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

        public IEnumerable CreateRunAllSequence()
        {
            foreach (var test in _store.Tests.Values)
            {
                foreach (var _ in CreateRunSequence(test.Id))
                {
                    yield return null;
                }
            }
        }

        public IEnumerable CreateRunSequence(string testId)
        {
            var test = _store.Tests[testId];
            return test.CreateRunAllSequence();
        }

        public IEnumerable CreateRunSequence(string testId, IReadOnlyList<string> entryIds)
        {
            var test = _store.Tests[testId];
            return test.CreateRunSequence(entryIds);
        }
    }
}

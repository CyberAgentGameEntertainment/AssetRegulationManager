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
        private readonly AssetRegulationManagerStore _store;

        public AssetRegulationTestExecuteService(AssetRegulationManagerStore store)
        {
            _store = store;
        }

        public IEnumerable CreateRunAllSequence(string testId)
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
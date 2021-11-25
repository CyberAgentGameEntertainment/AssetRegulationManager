// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class TestJob
    {
        private readonly RegulationViewerStore _store;

        internal TestJob(RegulationViewerStore store)
        {
            _store = store;
        }

        internal TestResultType Run(RegulationMetaDatum metaDatum, string path)
        {
            var regulation =
                _store.AssetRegulationCollection.Regulations.FirstOrDefault(x => x.Id == metaDatum.RegulationId);

            var entry = regulation?.Entries[metaDatum.EntryIndex];
            var obj = AssetDatabase.LoadAssetAtPath<Object>(path);

            var testResult = entry?.RunTest(obj) ?? false ? TestResultType.Success : TestResultType.Failed;

            return testResult;
        }
    }
}
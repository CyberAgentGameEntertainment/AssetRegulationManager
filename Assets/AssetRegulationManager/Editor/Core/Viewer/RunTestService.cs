// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class RunTestService
    {
        private readonly RegulationViewerStore _store;

        internal RunTestService(RegulationViewerStore store)
        {
            _store = store;
        }

        internal void Run(IEnumerable<RegulationMetaDatum> metaData)
        {
            _store.TestCollection.Value.Run(metaData);
        }
    }
}
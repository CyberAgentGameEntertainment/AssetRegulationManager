// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class AssetRegulationTestGenerateService
    {
        private readonly RegulationRegexFormatter _formatter;
        private readonly TestJob _job;
        private readonly RegulationViewerStore _store;

        internal AssetRegulationTestGenerateService(RegulationRegexFormatter formatter, TestJob job,
            RegulationViewerStore store)
        {
            _formatter = formatter;
            _job = job;
            _store = store;
        }

        internal void Run(string assetPathOrFilter)
        {
            _store.TestCollection.Value =
                new TestCollection(_job, _formatter.CreateRegulationViewData(assetPathOrFilter));
        }
    }
}
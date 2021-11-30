// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.Observable.ObservableCollection;

namespace AssetRegulationManager.Editor.Core.Model
{
    public class RegulationManagerStore
    {
        public RegulationManagerStore(List<AssetRegulation> regulations)
        {
            Regulations = regulations;
        }

        public List<AssetRegulation> Regulations { get; }

        public ObservableList<AssetRegulationTest> Tests { get; } =
            new ObservableList<AssetRegulationTest>();
    }
}
// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation.Observable.ObservableCollection;

namespace AssetRegulationManager.Editor.Core.Model
{
    internal sealed class AssetRegulationManagerStore
    {
        internal AssetRegulationManagerStore(List<AssetRegulation> regulations)
        {
            Regulations = regulations;
        }

        internal List<AssetRegulation> Regulations { get; }

        internal ObservableList<AssetRegulationTest> Tests { get; } =
            new ObservableList<AssetRegulationTest>();
    }
}
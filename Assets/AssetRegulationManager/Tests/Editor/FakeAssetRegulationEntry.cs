// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor
{
    public sealed class FakeAssetRegulationEntry : IAssetRegulationEntry
    {
        public FakeAssetRegulationEntry(bool result)
        {
            Result = result;
        }

        public bool Result { get; }
        public string Label => nameof(FakeAssetRegulationEntry);
        public string Description => nameof(FakeAssetRegulationEntry);

        public bool RunTest(Object obj)
        {
            return Result;
        }

        public void DrawGUI()
        {
        }
    }
}

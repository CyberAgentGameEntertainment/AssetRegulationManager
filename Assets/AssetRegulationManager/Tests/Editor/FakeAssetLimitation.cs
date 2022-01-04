// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.SelectableSerializeReference;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor
{
    [IgnoreSelectableSerializeReference]
    internal sealed class FakeAssetLimitation : IAssetLimitation
    {
        public FakeAssetLimitation(bool result)
        {
            Result = result;
        }

        public bool Result { get; }

        public string GetDescription()
        {
            return nameof(FakeAssetLimitation);
        }

        public bool Check(Object obj)
        {
            return Result;
        }
    }
}

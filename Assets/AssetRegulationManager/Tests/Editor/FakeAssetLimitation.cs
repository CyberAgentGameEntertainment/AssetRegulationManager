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
        private readonly string _description;

        public FakeAssetLimitation(bool result, string description = null)
        {
            Result = result;
            _description = description ?? nameof(FakeAssetLimitation);
        }

        public bool Result { get; }

        public string GetDescription()
        {
            return _description;
        }

        public string GetLatestValueAsText()
        {
            return string.Empty;
        }

        public bool Check(Object obj)
        {
            return Result;
        }
    }
}

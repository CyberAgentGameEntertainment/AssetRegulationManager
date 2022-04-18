// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Tests.Editor
{
    [IgnoreAssetConstraint]
    internal sealed class FakeAssetConstraint : IAssetConstraint
    {
        private readonly string _description;

        public FakeAssetConstraint(bool result, string description = null)
        {
            Result = result;
            _description = description ?? nameof(FakeAssetConstraint);
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

        public string Id { get; } = Guid.NewGuid().ToString();

        public bool Check(Object obj)
        {
            return Result;
        }
    }
}

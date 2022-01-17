// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Foundation.SelectableSerializeReference;
using UnityEngine;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl
{
    [Serializable]
    [SelectableSerializeReferenceLabel("Max Texture Size")]
    public sealed class MaxTextureSizeLimitation : AssetLimitation<Texture2D>
    {
        [SerializeField] private Vector2 _maxSize;
        private Vector2 _latestValue;

        public Vector2 MaxSize
        {
            get => _maxSize;
            set => _maxSize = value;
        }

        public override string GetDescription()
        {
            return $"Max Texture Size: {_maxSize.x} x {_maxSize.y}";
        }

        public override string GetLatestValueAsText()
        {
            return $"{_latestValue.x} x {_latestValue.y}";
        }

        /// <inheritdoc />
        protected override bool CheckInternal(Texture2D asset)
        {
            Assert.IsNotNull(asset);

            _latestValue = new Vector2(asset.width, asset.height);
            return asset.width <= _maxSize.x && asset.height <= _maxSize.y;
        }
    }
}

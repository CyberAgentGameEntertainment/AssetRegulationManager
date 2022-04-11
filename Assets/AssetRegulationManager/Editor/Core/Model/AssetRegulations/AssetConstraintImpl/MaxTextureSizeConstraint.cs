// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Foundation.EnabledIfAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("Texture/Max Texture Size", "Max Texture Size")]
    public sealed class MaxTextureSizeConstraint : AssetConstraint<Texture2D>
    {
        [SerializeField] private TextureSizeCountMode _countMode;

        [SerializeField] [EnabledIf("_countMode", 0, HideMode.Invisible)]
        private Vector2 _maxSize;

        [SerializeField] [EnabledIf("_countMode", 1, HideMode.Invisible)]
        private int _maxTexelCount;

        private Vector2 _latestValue;

        public TextureSizeCountMode CountMode
        {
            get => _countMode;
            set => _countMode = value;
        }

        public Vector2 MaxSize
        {
            get => _maxSize;
            set => _maxSize = value;
        }

        public int MaxTexelCount
        {
            get => _maxTexelCount;
            set => _maxTexelCount = value;
        }

        public override string GetDescription()
        {
            switch (CountMode)
            {
                case TextureSizeCountMode.WidthAndHeight:
                    return $"Max Texture Size: {_maxSize.x} x {_maxSize.y}";
                case TextureSizeCountMode.TexelCount:
                    return $"Max Texel Count: {_maxTexelCount}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string GetLatestValueAsText()
        {
            switch (CountMode)
            {
                case TextureSizeCountMode.WidthAndHeight:
                    return $"{_latestValue.x} x {_latestValue.y}";
                case TextureSizeCountMode.TexelCount:
                    return $"{_latestValue.x * _latestValue.y}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <inheritdoc />
        protected override bool CheckInternal(Texture2D asset)
        {
            Assert.IsNotNull(asset);

            switch (CountMode)
            {
                case TextureSizeCountMode.WidthAndHeight:
                    _latestValue = new Vector2(asset.width, asset.height);
                    return asset.width <= _maxSize.x && asset.height <= _maxSize.y;
                case TextureSizeCountMode.TexelCount:
                    return asset.width * asset.height <= _maxTexelCount;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

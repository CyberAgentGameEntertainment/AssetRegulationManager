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

        public Vector2 MaxSize
        {
            set => _maxSize = value;
            get => _maxSize;
        }

        public override string GetDescription()
        {
            return $"Max Texture Size: {_maxSize.x} x {_maxSize.y}";
        }

        /// <inheritdoc />
        protected override bool CheckInternal(Texture2D asset)
        {
            Assert.IsNotNull(asset);

            return asset.width <= _maxSize.x && asset.height <= _maxSize.y;
        }
    }
}

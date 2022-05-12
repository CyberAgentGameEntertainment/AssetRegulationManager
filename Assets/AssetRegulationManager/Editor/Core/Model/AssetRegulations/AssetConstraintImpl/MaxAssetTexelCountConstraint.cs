using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("Texture/Max Texel Count in Asset", "Max Texel Count in Asset")]
    public sealed class MaxAssetTexelCountConstraint : AssetConstraint<Object>
    {
        [SerializeField] private int _maxCount;

        private int _latestValue;

        public int MaxCount
        {
            get => _maxCount;
            set => _maxCount = value;
        }

        public override string GetDescription()
        {
            return $"Max Asset Texel Count: {_maxCount}";
        }

        public override string GetLatestValueAsText()
        {
            return $"{_latestValue}";
        }

        /// <inheritdoc />
        protected override bool CheckInternal(Object asset)
        {
            Assert.IsNotNull(asset);

            var textures = EditorUtility.CollectDependencies(new[] { asset })
                .OfType<Texture>();

            var texelCount = 0;
            foreach (var texture in textures)
                texelCount += texture.width * texture.height;

            _latestValue = texelCount;
            return texelCount <= _maxCount;
        }
    }
}

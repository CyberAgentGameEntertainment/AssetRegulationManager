// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Linq;
using AssetRegulationManager.Editor.Foundation.SelectableSerializeReference;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl
{
    [Serializable]
    [SelectableSerializeReferenceLabel("Max Texel Count (Game Object)")]
    public sealed class MaxGameObjectTexelCountLimitation : AssetLimitation<GameObject>
    {
        [SerializeField] private int _maxTexelCount;
        [SerializeField] private bool _excludeChildren;
        [SerializeField] private bool _excludeInactive;
        [SerializeField] private bool _allowDuplicateCount;

        private int _latestValue;

        public int MaxTexelCount
        {
            get => _maxTexelCount;
            set => _maxTexelCount = value;
        }

        public bool ExcludeChildren
        {
            get => _excludeChildren;
            set => _excludeChildren = value;
        }

        public bool ExcludeInactive
        {
            get => _excludeInactive;
            set => _excludeInactive = value;
        }

        public bool AllowDuplicateCount
        {
            get => _allowDuplicateCount;
            set => _allowDuplicateCount = value;
        }

        public override string GetDescription()
        {
            return $"Max Texel Count: {_maxTexelCount}";
        }

        public override string GetLatestValueAsText()
        {
            return $"{_latestValue}";
        }

        /// <inheritdoc />
        protected override bool CheckInternal(GameObject asset)
        {
            Assert.IsNotNull(asset);

            var renderers = _excludeChildren
                ? asset.GetComponents<Renderer>()
                : asset.GetComponentsInChildren<Renderer>(!_excludeInactive);
            var materials = renderers.SelectMany(x => x.sharedMaterials);
            var textures = materials
                .SelectMany(x => EditorUtility.CollectDependencies(new Object[] { x }))
                .OfType<Texture>();

            if (!_allowDuplicateCount)
            {
                textures = textures.Distinct();
            }

            var texelCount = 0;
            foreach (var texture in textures)
            {
                texelCount += texture.width * texture.height;
            }

            _latestValue = texelCount;
            return texelCount <= _maxTexelCount;
        }
    }
}

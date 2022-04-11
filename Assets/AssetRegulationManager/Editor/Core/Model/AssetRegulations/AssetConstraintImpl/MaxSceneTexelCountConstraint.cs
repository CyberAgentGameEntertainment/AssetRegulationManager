using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("Texture/Max Texel Count in Scene", "Max Texel Count in Scene")]
    public sealed class MaxSceneTexelCountConstraint : AssetConstraint<SceneAsset>
    {
        [SerializeField] private int _maxCount;
        [SerializeField] private bool _excludeInactive;
        [SerializeField] private bool _allowDuplicateCount;

        private int _latestValue;

        public int MaxCount
        {
            get => _maxCount;
            set => _maxCount = value;
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
            var optionsDescription = new StringBuilder();
            optionsDescription.Append("(");
            optionsDescription.Append(_excludeInactive ? "Exclude Inactive" : "Include Inactive");
            optionsDescription.Append(", ");
            optionsDescription.Append(_allowDuplicateCount ? "Allow Duplicate Count" : "Disallow Duplicate Count");
            optionsDescription.Append(")");
            var desc = $"Max Texel Count in Scene: {_maxCount} {optionsDescription}";
            return desc;
        }

        public override string GetLatestValueAsText()
        {
            return _latestValue.ToString();
        }

        protected override bool CheckInternal(SceneAsset asset)
        {
            Assert.IsNotNull(asset);

            var sceneAssetPath = AssetDatabase.GetAssetPath(asset);
            if (!AssetConstraintUtility.OpenScene(sceneAssetPath))
            {
                throw new Exception("The process was canceled by user operation.");
            }

            var renderers = AssetConstraintUtility.GetAllComponentsInActiveScene<Renderer>(!_excludeInactive);
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
            return texelCount <= _maxCount;
        }
    }
}

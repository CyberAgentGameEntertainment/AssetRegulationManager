using System;
using System.Linq;
using AssetRegulationManager.Editor.Core.Shared;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("Texture/Max Texel Count in Folder", "Max Texel Count in Folder")]
    public sealed class MaxFolderTexelCountConstraint : AssetConstraint<DefaultAsset>
    {
        [SerializeField] private int _maxCount;
        [SerializeField] private bool _topFolderOnly;

        private int _latestValue;

        public int MaxCount
        {
            get => _maxCount;
            set => _maxCount = value;
        }

        public bool TopFolderOnly
        {
            get => _topFolderOnly;
            set => _topFolderOnly = value;
        }

        public override string GetDescription()
        {
            var desc = $"Max Texel Count in Folder: {_maxCount}";
            return desc;
        }

        public override string GetLatestValueAsText()
        {
            return _latestValue.ToString();
        }

        protected override bool CheckInternal(DefaultAsset asset)
        {
            Assert.IsNotNull(asset);

            var assetPath = AssetDatabase.GetAssetPath(asset);
            if (!AssetDatabase.IsValidFolder(assetPath))
                throw new ArgumentException($"Invalid Type: {asset.GetType()} is not folder.");

            var textures = AssetDatabase.FindAssets("t:Texture", new[] { assetPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(x =>
                {
                    if (_topFolderOnly)
                    {
                        var parentFolderPath = AssetPathUtility.GetFolderPath(x);
                        return parentFolderPath == assetPath;
                    }

                    return true;
                })
                .Select(AssetDatabase.LoadAssetAtPath<Texture>);

            var texelCount = 0;
            foreach (var texture in textures)
                texelCount += texture.width * texture.height;

            _latestValue = texelCount;
            return texelCount <= _maxCount;
        }
    }
}

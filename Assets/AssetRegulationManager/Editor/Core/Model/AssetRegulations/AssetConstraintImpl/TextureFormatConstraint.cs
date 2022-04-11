// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Text;
using AssetRegulationManager.Editor.Core.Shared.ListableProperties;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("Texture/Texture Format", "Texture Format")]
    public sealed class TextureFormatConstraint : AssetConstraint<Texture2D>
    {
        [SerializeField] private BuildTargetGroupListableProperty _target = new BuildTargetGroupListableProperty();

        [SerializeField]
        private TextureImporterFormatListableProperty _format = new TextureImporterFormatListableProperty();

        public BuildTargetGroupListableProperty Target => _target;

        public TextureImporterFormatListableProperty Format => _format;

        private TextureImporterFormat? _latestValue;

        public override string GetDescription()
        {
            var formats = new StringBuilder();
            var formatCount = 0;
            foreach (var format in _format)
            {
                if (!Enum.IsDefined(typeof(TextureImporterFormat), format))
                {
                    continue;
                }

                if (formatCount >= 1)
                {
                    formats.Append(" || ");
                }

                formats.Append(format.ToString());
                formatCount++;
            }

            if (formatCount >= 2)
            {
                formats.Insert(0, "( ");
                formats.Append(" )");
            }

            var targets = new StringBuilder();
            var targetCount = 0;
            foreach (var target in _target)
            {
                if (!Enum.IsDefined(typeof(BuildTargetGroup), target))
                {
                    continue;
                }

                if (targetCount >= 1)
                {
                    targets.Append(", ");
                }

                targets.Append(target.ToString());
                targetCount++;
            }

            if (targetCount >= 2)
            {
                targets.Insert(0, "( ");
                targets.Append(" )");
            }

            return $"Texture Format: {formats} in {targets}";
        }

        public override string GetLatestValueAsText()
        {
            return _latestValue == null ? "None" : _latestValue.ToString();
        }

        /// <inheritdoc />
        protected override bool CheckInternal(Texture2D asset)
        {
            Assert.IsNotNull(asset);

            var assetPath = AssetDatabase.GetAssetPath(asset);
            var importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);

            foreach (var target in _target)
            {
                if (!Enum.IsDefined(typeof(BuildTargetGroup), target))
                {
                    continue;
                }

                var targetString = target.ToString();
                var assetFormat = importer.GetPlatformTextureSettings(targetString).format;
                _latestValue = assetFormat;

                foreach (var format in _format)
                {
                    if (!Enum.IsDefined(typeof(TextureImporterFormat), format))
                    {
                        continue;
                    }

                    if (format == assetFormat)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

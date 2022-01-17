// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Text;
using AssetRegulationManager.Editor.Core.Tool.Shared.ListableProperties;
using AssetRegulationManager.Editor.Foundation.SelectableSerializeReference;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl
{
    [Serializable]
    [SelectableSerializeReferenceLabel("Texture Format")]
    public sealed class TextureFormatLimitation : AssetLimitation<Texture2D>
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
            foreach (var format in _format)
            {
                if (!Enum.IsDefined(typeof(TextureImporterFormat), format))
                {
                    continue;
                }

                if (formats.Length != 0)
                {
                    formats.Append(", ");
                }

                formats.Append(format.ToString());
            }

            var targets = new StringBuilder();
            foreach (var target in _target)
            {
                if (!Enum.IsDefined(typeof(BuildTargetGroup), target))
                {
                    continue;
                }

                if (targets.Length != 0)
                {
                    targets.Append(", ");
                }

                targets.Append(target.ToString());
            }

            return $"Texture Format: {formats} ({targets})";
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

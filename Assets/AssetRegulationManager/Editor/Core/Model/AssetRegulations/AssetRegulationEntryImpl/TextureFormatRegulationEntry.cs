// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetRegulationEntryImpl
{
    /// <summary>
    ///     Texture format regulation class.
    /// </summary>
    [Serializable]
    public class TextureFormatRegulationEntry : AssetRegulationEntry<Texture2D>
    {
        [SerializeField] private BuildTargetGroup _target;
        [SerializeField] private List<TextureImporterFormat> _formats = new List<TextureImporterFormat>();

        public BuildTargetGroup Target
        {
            get => _target;
            set => _target = value;
        }

        public List<TextureImporterFormat> Formats => _formats;

        public override string Label => "Texture Format";

        public override string Description
        {
            get
            {
                var formats = new StringBuilder();
                foreach (var format in _formats)
                {
                    if (formats.Length != 0)
                    {
                        formats.Append(", ");
                    }

                    formats.Append(format.ToString());
                }

                return $"Texture Format: {formats}";
            }
        }

        /// <summary>
        ///     Determine if the IOS setting for texture type is correct
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        protected override bool RunTest(Texture2D asset)
        {
            Assert.IsNotNull(asset);
            var assetPath = AssetDatabase.GetAssetPath(asset);
            var importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);
            var targetString = _target.ToString();
            var format = importer.GetPlatformTextureSettings(targetString).format;
            return Formats.Contains(format);
        }

        public override void DrawGUI()
        {
            // TODO: 実装する
        }
    }
}

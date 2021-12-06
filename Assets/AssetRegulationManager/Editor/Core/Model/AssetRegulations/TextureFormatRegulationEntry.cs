// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    /// <summary>
    ///     Texture size regulation class.
    /// </summary>
    public class TextureFormatRegulationEntry : AssetRegulationEntry<Texture2D>
    {
        [SerializeField] private TextureFormat _textureFormat;

        /// <summary>
        ///     Initialize.
        /// </summary>
        /// <param name="textureFormat"></param>
        public TextureFormatRegulationEntry(TextureFormat textureFormat)
        {
            _textureFormat = textureFormat;
        }

        /// <summary>
        ///     Texture Format Regulation.
        /// </summary>
        public TextureFormat TextureFormat
        {
            set => _textureFormat = value;
            get => _textureFormat;
        }

        public override string Label => "Texture Size";
        public override string Description => $"Texture format must be {_textureFormat}";

        private static string PlatformName = "iPhone";

        public override void DrawGUI()
        {
            // TODO: 実装する
        }

        /// <summary>
        ///     Determine if the IOS setting for texture type is correct
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        protected override bool RunTest(Texture2D asset)
        {
            var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(asset)) as TextureImporter;
            
            if (importer == null)
                return false;
            
            var importerFormat = importer.GetAutomaticFormat(PlatformName);

            if (importerFormat == TextureImporterFormat.Automatic)
                return importer.GetPlatformTextureSettings(PlatformName).format.ToString() == TextureFormat.ToString();
            
            return importerFormat.ToString() == TextureFormat.ToString();
        }
    }
}
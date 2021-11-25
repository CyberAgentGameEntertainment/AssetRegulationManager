// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEngine;

namespace AssetRegulationManager.Editor.Core
{
    /// <summary>
    ///     Texture size regulation class.
    /// </summary>
    public class TextureSizeRegulationEntry : AssetRegulationEntry<Texture2D>
    {
        [SerializeField] private Vector2 _textureSize;

        /// <summary>
        ///     Initialize.
        /// </summary>
        /// <param name="textureSize"></param>
        public TextureSizeRegulationEntry(Vector2 textureSize)
        {
            _textureSize = textureSize;
        }

        /// <summary>
        ///     Texture Size Regulation.
        /// </summary>
        public Vector2 TextureSize
        {
            set => _textureSize = value;
            get => _textureSize;
        }

        public override string Label => "Texture Size";
        public override string Explanation => $"Texture size must be less than({_textureSize.x}x{_textureSize.y})";

        public override void DrawGUI()
        {
            // TODO: 実装する
        }

        /// <summary>
        ///     Determine if the texture size is the regulation value or less.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        protected override bool RunTest(Texture2D asset)
        {
            return asset.width <= _textureSize.x && asset.height <= _textureSize.y;
        }
    }
}
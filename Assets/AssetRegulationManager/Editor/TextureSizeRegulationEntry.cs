// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEngine;

namespace AssetRegulationManager.Editor
{
    public class TextureSizeRegulationEntry : AssetRegulationEntry<Texture2D>
    {
        [SerializeField] private Vector2 _textureSize;
        public Vector2 TextureSize
        {
            set => _textureSize = value;
            get => _textureSize;
        }

        public TextureSizeRegulationEntry(Vector2 textureSize)
        {
            _textureSize = textureSize;
        }

        public override string Label => "Texture Size";
        public override string Explanation => $"Texture size must be less than({_textureSize.x}x{_textureSize.y})";

        public override void DrawGUI()
        {
            // TODO: 実装する
        }

        protected override bool RunTest(Texture2D asset)
        {
            return asset.width <= _textureSize.x && asset.height <= _textureSize.y;
        }
    }
}
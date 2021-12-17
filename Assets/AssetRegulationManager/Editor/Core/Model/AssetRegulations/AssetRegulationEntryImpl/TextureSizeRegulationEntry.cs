// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetRegulationEntryImpl
{
    /// <summary>
    ///     Texture size regulation class.
    /// </summary>
    [Serializable]
    public class TextureSizeRegulationEntry : AssetRegulationEntry<Texture2D>
    {
        [SerializeField] private Vector2 _maxSize;

        /// <summary>
        ///     Texture Size Regulation.
        /// </summary>
        public Vector2 MaxSize
        {
            set => _maxSize = value;
            get => _maxSize;
        }

        public override string Label => "Texture Size";
        public override string Description => $"Texture Size: ({_maxSize.x}x{_maxSize.y})";

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
            return asset.width <= _maxSize.x && asset.height <= _maxSize.y;
        }
    }
}
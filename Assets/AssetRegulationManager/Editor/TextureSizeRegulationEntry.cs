using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor
{
    public class TextureSizeRegulationEntry : IAssetRegulationEntry
    {
        [SerializeField] private Vector2 _textureSize;
        public string Label => "Texture Size";
        public string Explanation => $"Texture size must be less than({_textureSize.x}x{_textureSize.y})";

        public bool RunTest(string path)
        {
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            return texture.width <= _textureSize.x && texture.height <= _textureSize.y;
        }

        public void DrawGUI()
        {
            // TODO: 実装する
        }
    }
}
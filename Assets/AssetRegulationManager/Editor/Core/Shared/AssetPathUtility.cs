using System.IO;

namespace AssetRegulationManager.Editor.Core.Shared
{
    public static class AssetPathUtility
    {
        internal static string GetFolderPath(string assetPath)
        {
            var result = Path.GetDirectoryName(assetPath);
            return NormalizeAssetPath(result);
        }

        internal static string GetFolderName(string assetPath)
        {
            var folderPath = GetFolderPath(assetPath);
            if (string.IsNullOrEmpty(folderPath))
                return string.Empty;

            var split = folderPath.Split('/');
            return split[split.Length - 1];
        }

        internal static string NormalizeAssetPath(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return string.Empty;

            return assetPath.Replace('\\', '/');
        }
    }
}

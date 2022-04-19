namespace AssetRegulationManager.Editor.Core.Shared
{
    public static class AssetPathUtility
    {
        internal static string NormalizeAssetPath(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return string.Empty;

            return assetPath.Replace('\\', '/');
        }
    }
}

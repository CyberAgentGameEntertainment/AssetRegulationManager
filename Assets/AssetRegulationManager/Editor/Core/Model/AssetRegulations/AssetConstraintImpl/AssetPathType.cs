using System;
using System.IO;
using AssetRegulationManager.Editor.Core.Shared;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    public enum AssetPathType
    {
        AssetPath,
        AssetName,
        AssetNameWithoutExtensions,
        FolderName,
        FolderPath
    }

    public static class AssetPathModeExtensions
    {
        public static string ConvertAssetPath(this AssetPathType assetPathType, string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return string.Empty;

            assetPath = AssetPathUtility.NormalizeAssetPath(assetPath);

            switch (assetPathType)
            {
                case AssetPathType.AssetPath:
                    return assetPath;
                case AssetPathType.AssetName:
                    return Path.GetFileName(assetPath);
                case AssetPathType.AssetNameWithoutExtensions:
                    return Path.GetFileNameWithoutExtension(assetPath);
                case AssetPathType.FolderName:
                    return AssetPathUtility.GetFolderName(assetPath);
                case AssetPathType.FolderPath:
                    return AssetPathUtility.GetFolderPath(assetPath);
                default:
                    throw new ArgumentOutOfRangeException(nameof(assetPathType), assetPathType, null);
            }
        }
    }
}

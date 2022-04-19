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

            string result;
            switch (assetPathType)
            {
                case AssetPathType.AssetPath:
                    result = assetPath;
                    break;
                case AssetPathType.AssetName:
                    result = Path.GetFileName(assetPath);
                    break;
                case AssetPathType.AssetNameWithoutExtensions:
                    result = Path.GetFileNameWithoutExtension(assetPath);
                    break;
                case AssetPathType.FolderName:
                    result = Path.GetDirectoryName(assetPath);
                    if (string.IsNullOrEmpty(result))
                        break;
                    var split = result.Split('/');
                    result = split[split.Length - 1];
                    break;
                case AssetPathType.FolderPath:
                    result = Path.GetDirectoryName(assetPath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(assetPathType), assetPathType, null);
            }

            return AssetPathUtility.NormalizeAssetPath(result);
        }
    }
}

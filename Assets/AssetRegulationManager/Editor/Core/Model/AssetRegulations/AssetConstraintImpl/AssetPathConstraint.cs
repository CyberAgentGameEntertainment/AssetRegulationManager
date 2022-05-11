using System;
using System.Text;
using System.Text.RegularExpressions;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("File/Asset Path", "Asset Path")]
    public sealed class AssetPathConstraint : AssetConstraint<Object>
    {
        [SerializeField] private AssetPathType _pathType = AssetPathType.AssetPath;
        [SerializeField] private AssetPathConstraintCheckMode _checkMode = AssetPathConstraintCheckMode.Or;
        [SerializeField] private StringListableProperty _assetPath = new StringListableProperty();

        private string _latestValue;

        public StringListableProperty AssetPath => _assetPath;

        public AssetPathType PathType
        {
            get => _pathType;
            set => _pathType = value;
        }

        public AssetPathConstraintCheckMode CheckMode
        {
            get => _checkMode;
            set => _checkMode = value;
        }

        public override string GetDescription()
        {
            var assetPaths = new StringBuilder();
            var assetPathCount = 0;
            foreach (var path in _assetPath)
            {
                if (string.IsNullOrEmpty(path))
                    continue;

                if (assetPathCount >= 1)
                {
                    var delimiter = _checkMode == AssetPathConstraintCheckMode.And ? " && " : " || ";
                    assetPaths.Append(delimiter);
                }

                assetPaths.Append(path);
                assetPathCount++;
            }

            if (assetPathCount >= 2)
            {
                assetPaths.Insert(0, "( ");
                assetPaths.Append(" )");
            }

            string label;
            switch (_pathType)
            {
                case AssetPathType.AssetPath:
                    label = "Asset Path";
                    break;
                case AssetPathType.AssetName:
                    label = "Asset Name";
                    break;
                case AssetPathType.AssetNameWithoutExtensions:
                    label = "Asset Name (Without Extensions)";
                    break;
                case AssetPathType.FolderName:
                    label = "Folder Name";
                    break;
                case AssetPathType.FolderPath:
                    label = "Folder Path";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return $"{label}: {assetPaths}";
        }

        public override string GetLatestValueAsText()
        {
            return string.IsNullOrEmpty(_latestValue) ? "None" : _latestValue;
        }

        /// <inheritdoc />
        protected override bool CheckInternal(Object asset)
        {
            Assert.IsNotNull(asset);

            var assetPath = AssetDatabase.GetAssetPath(asset);
            assetPath = _pathType.ConvertAssetPath(assetPath);
            _latestValue = assetPath;

            switch (CheckMode)
            {
                case AssetPathConstraintCheckMode.Or:
                    foreach (var path in _assetPath)
                    {
                        if (string.IsNullOrEmpty(path))
                            continue;

                        var regex = new Regex(path);
                        if (regex.IsMatch(assetPath))
                            return true;
                    }

                    return false;
                case AssetPathConstraintCheckMode.And:
                    foreach (var path in _assetPath)
                    {
                        if (string.IsNullOrEmpty(path))
                            continue;

                        var regex = new Regex(path);
                        if (!regex.IsMatch(assetPath))
                            return false;
                    }

                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

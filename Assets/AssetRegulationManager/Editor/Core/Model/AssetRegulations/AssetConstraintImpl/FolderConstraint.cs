using System;
using System.IO;
using System.Text;
using AssetRegulationManager.Editor.Core.Shared;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("Path/Folder", "Folder")]
    public sealed class FolderConstraint : AssetConstraint<Object>
    {
        [SerializeField] private bool _topFolderOnly;
        [SerializeField] private FolderConstraintCheckMode _checkMode = FolderConstraintCheckMode.Contains;
        [SerializeField] private ObjectListableProperty _folder = new ObjectListableProperty();

        private string _latestValue;

        public ObjectListableProperty Folder => _folder;

        public bool TopDirectoryOnly
        {
            get => _topFolderOnly;
            set => _topFolderOnly = value;
        }

        public FolderConstraintCheckMode CheckMode
        {
            get => _checkMode;
            set => _checkMode = value;
        }

        public override string GetDescription()
        {
            var folderPath = new StringBuilder();
            var folderPathCount = 0;
            foreach (var folder in _folder)
            {
                if (folder == null)
                    continue;

                var folderAssetPath = AssetDatabase.GetAssetPath(folder);

                if (folderPathCount >= 1)
                {
                    var delimiter = " || ";
                    folderPath.Append(delimiter);
                }

                folderPath.Append(folderAssetPath);
                folderPathCount++;
            }

            if (folderPathCount >= 2)
            {
                folderPath.Insert(0, "( ");
                folderPath.Append(" )");
            }

            string label;
            switch (CheckMode)
            {
                case FolderConstraintCheckMode.Contains:
                    label = "Contained in Folder";
                    break;
                case FolderConstraintCheckMode.NotContains:
                    label = "Not Contained in Folder";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_topFolderOnly) label += " (Top Folder Only)";
            return $"{label}: {folderPath}";
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
            _latestValue = assetPath;
            var assetFolderPath = Path.GetDirectoryName(assetPath);
            assetFolderPath = AssetPathUtility.NormalizeAssetPath(assetFolderPath);
            if (string.IsNullOrEmpty(assetFolderPath)) return _checkMode == FolderConstraintCheckMode.NotContains;

            switch (CheckMode)
            {
                case FolderConstraintCheckMode.Contains:
                    foreach (var folder in _folder)
                    {
                        if (folder == null)
                            continue;

                        var folderPath = AssetDatabase.GetAssetPath(folder);
                        if (!AssetDatabase.IsValidFolder(folderPath))
                            continue;

                        if (_topFolderOnly)
                        {
                            if (folderPath == assetFolderPath)
                                return true;
                        }
                        else
                        {
                            if (assetFolderPath.Contains(folderPath))
                                return true;
                        }
                    }

                    return false;
                case FolderConstraintCheckMode.NotContains:
                    foreach (var folder in _folder)
                    {
                        if (folder == null)
                            continue;

                        var folderPath = AssetDatabase.GetAssetPath(folder);
                        if (!AssetDatabase.IsValidFolder(folderPath))
                            continue;

                        if (_topFolderOnly)
                        {
                            if (folderPath != assetFolderPath)
                                return true;
                        }
                        else
                        {
                            if (!assetFolderPath.Contains(folderPath))
                                return true;
                        }
                    }

                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

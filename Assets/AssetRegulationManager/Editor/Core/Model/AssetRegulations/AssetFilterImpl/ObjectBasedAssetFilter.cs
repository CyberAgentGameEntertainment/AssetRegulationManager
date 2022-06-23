// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl
{
    /// <summary>
    ///     Filter to pass assets if matches or contained in the folder.
    /// </summary>
    [Serializable]
    [AssetFilter("Object Filter", "Object Filter")]
    public sealed class ObjectBasedAssetFilter : AssetFilterBase
    {
        [SerializeField] private FolderTargetingMode _folderTargetingMode = FolderTargetingMode.IncludedNonFolderAssets;
        [SerializeField] private ObjectListableProperty _object = new ObjectListableProperty();
        private List<string> _assetPaths = new List<string>();

        private List<bool> _folderFlags = new List<bool>();

        public FolderTargetingMode FolderTargetingMode
        {
            get => _folderTargetingMode;
            set => _folderTargetingMode = value;
        }

        /// <summary>
        ///     Objects for filtering.
        /// </summary>
        public ObjectListableProperty Object => _object;

        public override void SetupForMatching()
        {
            _folderFlags.Clear();
            _assetPaths.Clear();
            foreach (var obj in _object)
            {
                if (obj == null)
                    continue;

                var isFolder = obj is DefaultAsset;
                _folderFlags.Add(isFolder);

                var path = AssetDatabase.GetAssetPath(obj);
                _assetPaths.Add(path);
            }
        }

        /// <inheritdoc />
        public override bool IsMatch(string assetPath, Type assetType, bool isFolder)
        {
            if (string.IsNullOrEmpty(assetPath))
                return false;

            for (var i = 0; i < _assetPaths.Count; i++)
            {
                var isSelf = _assetPaths[i] == assetPath;
                if (_folderFlags[i])
                {
                    var isInclusion = !isSelf && !isFolder &&
                                      assetPath.StartsWith(_assetPaths[i], StringComparison.Ordinal);
                    switch (FolderTargetingMode)
                    {
                        case FolderTargetingMode.IncludedNonFolderAssets:
                            if (isInclusion)
                                return true;
                            break;
                        case FolderTargetingMode.Self:
                            if (isSelf)
                                return true;
                            break;
                        case FolderTargetingMode.Both:
                            if (isInclusion || isSelf)
                                return true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    if (isSelf)
                        return true;
                }
            }

            return false;
        }

        public override string GetDescription()
        {
            var result = new StringBuilder();
            var elementCount = 0;
            foreach (var obj in _object)
            {
                if (obj == null) continue;

                if (elementCount >= 1) result.Append(" || ");

                var path = AssetDatabase.GetAssetPath(obj);
                result.Append(Path.GetFileNameWithoutExtension(path));
                elementCount++;
            }

            if (result.Length >= 1)
            {
                if (elementCount >= 2)
                {
                    result.Insert(0, "( ");
                    result.Append(" )");
                }

                result.Insert(0, "Object: ");
            }

            return result.ToString();
        }
    }
}

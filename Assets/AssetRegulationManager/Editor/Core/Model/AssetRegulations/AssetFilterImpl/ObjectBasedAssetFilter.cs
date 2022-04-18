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
        [SerializeField] private ObjectListableProperty _object = new ObjectListableProperty();

        private List<string> _assetPaths = new List<string>();

        /// <summary>
        ///     Objects for filtering.
        /// </summary>
        public ObjectListableProperty Object => _object;

        public override void SetupForMatching()
        {
            _assetPaths.Clear();
            foreach (var obj in _object)
            {
                if (obj == null)
                {
                    continue;
                }

                var path = AssetDatabase.GetAssetPath(obj);
                _assetPaths.Add(path);
            }
        }

        /// <inheritdoc />
        public override bool IsMatch(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return false;
            }

            foreach (var path in _assetPaths)
            {
                // Return true if any of the asset or folder match.
                if (assetPath.Contains(path))
                {
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
                if (obj == null)
                {
                    continue;
                }

                if (elementCount >= 1)
                {
                    result.Append(" || ");
                }

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

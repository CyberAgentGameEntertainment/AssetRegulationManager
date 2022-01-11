// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using AssetRegulationManager.Editor.Foundation.SelectableSerializeReference;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl
{
    /// <summary>
    ///     Filter to pass assets if matches or contained in the folder.
    /// </summary>
    [Serializable]
    [SelectableSerializeReferenceLabel("Asset or Folder")]
    public sealed class ObjectBasedAssetFilter : IAssetFilter
    {
        [SerializeField] private ObjectListableProperty _object = new ObjectListableProperty();

        private List<string> _assetPaths = new List<string>();

        /// <summary>
        ///     Objects for filtering.
        /// </summary>
        public ObjectListableProperty Object => _object;

        public void SetupForMatching()
        {
            // In Unity2019.4.13 and earlier or Unity2020.1.0-2020.1.16, this field can be null when deserialization.
            // This is a Unity's bug; issue id is 1253433.
            // The following null check is a work-around for this.
            if (_assetPaths == null)
            {
                _assetPaths = new List<string>();
            }

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
        public bool IsMatch(string assetPath)
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

        public string GetDescription()
        {
            var result = new StringBuilder();
            var isFirstItem = true;
            foreach (var obj in _object)
            {
                if (obj == null)
                {
                    continue;
                }

                if (!isFirstItem)
                {
                    result.Append(", ");
                }

                var path = AssetDatabase.GetAssetPath(obj);
                result.Append(Path.GetFileNameWithoutExtension(path));

                isFirstItem = false;
            }

            if (result.Length >= 1)
            {
                result.Insert(0, "Objects: ");
            }

            return result.ToString();
        }
    }
}

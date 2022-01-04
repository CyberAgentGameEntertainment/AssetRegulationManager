// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using AssetRegulationManager.Editor.Foundation.SelectableSerializeReference;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl
{
    /// <summary>
    ///     Filter to pass assets if their extensions match.
    /// </summary>
    [Serializable]
    [SelectableSerializeReferenceLabel("Extension")]
    public sealed class ExtensionBasedAssetFilter : IAssetFilter
    {
        [SerializeField] private StringListableProperty _extension = new StringListableProperty();
        private List<string> _extensions = new List<string>();

        /// <summary>
        ///     Extensions for filtering.
        /// </summary>
        public StringListableProperty Extension => _extension;

        public void SetupForMatching()
        {
            _extensions.Clear();
            foreach (var extension in _extension)
            {
                if (string.IsNullOrEmpty(extension))
                {
                    continue;
                }

                var ext = extension;
                if (!ext.StartsWith("."))
                {
                    ext = $".{extension}";
                }

                _extensions.Add(ext);
            }
        }

        /// <inheritdoc />
        public bool IsMatch(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return false;
            }

            var targetExtension = Path.GetExtension(assetPath);
            if (string.IsNullOrEmpty(targetExtension))
            {
                return false;
            }

            foreach (var extension in _extensions)
            {
                // Return true if any of the extensions match.
                if (extension == targetExtension)
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
            foreach (var extension in _extension)
            {
                if (string.IsNullOrEmpty(extension))
                {
                    continue;
                }

                if (!isFirstItem)
                {
                    result.Append(", ");
                }

                result.Append(extension);
                isFirstItem = false;
            }

            if (result.Length >= 1)
            {
                result.Insert(0, "Extensions: ");
            }

            return result.ToString();
        }
    }
}

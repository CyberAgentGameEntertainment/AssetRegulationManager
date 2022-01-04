// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using AssetRegulationManager.Editor.Foundation.SelectableSerializeReference;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    /// <summary>
    ///     Define a group of multiple assets.
    /// </summary>
    [Serializable]
    public sealed class AssetGroup
    {
        [SerializeReference] [SelectableSerializeReference(true)]
        private List<IAssetFilter> _filters = new List<IAssetFilter>();

        /// <summary>
        ///     Filter to determine whether an asset belongs to this group.
        /// </summary>
        public List<IAssetFilter> Filters => _filters;

        public void Setup()
        {
            foreach (var filter in _filters)
            {
                filter?.SetupForMatching();
            }
        }

        /// <summary>
        ///     Return true if the <see cref="assetPath" /> asset belongs to this group.
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public bool Contains(string assetPath)
        {
            if (_filters.Count == 0)
            {
                return false;
            }

            foreach (var filter in _filters)
            {
                if (filter == null)
                {
                    continue;
                }

                if (!filter.IsMatch(assetPath))
                {
                    return false;
                }
            }

            return true;
        }

        public string GetDescription()
        {
            var result = new StringBuilder();
            var isFirstItem = true;
            foreach (var filter in _filters)
            {
                if (filter == null)
                {
                    continue;
                }

                var description = filter.GetDescription();
                if (string.IsNullOrEmpty(description))
                {
                    continue;
                }

                if (!isFirstItem)
                {
                    result.Append(", ");
                }

                result.Append(description);
                isFirstItem = false;
            }

            return result.Length >= 1 ? result.ToString() : "None";
        }
    }
}

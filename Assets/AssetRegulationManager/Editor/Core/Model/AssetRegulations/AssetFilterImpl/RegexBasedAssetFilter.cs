﻿// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using AssetRegulationManager.Editor.Foundation.SelectableSerializeReference;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl
{
    /// <summary>
    ///     Filter to pass assets if matches the regex.
    /// </summary>
    [Serializable]
    [SelectableSerializeReferenceLabel("Regex")]
    public sealed class RegexBasedAssetFilter : IAssetFilter
    {
        [SerializeField] private StringListableProperty _assetPathRegex = new StringListableProperty();
        private List<Regex> _regexes = new List<Regex>();

        /// <summary>
        ///     Regex string.
        /// </summary>
        public StringListableProperty AssetPathRegex => _assetPathRegex;

        public void SetupForMatching()
        {
            // In Unity2019.4.13 and earlier or Unity2020.1.0-2020.1.16, this field can be null when deserialization.
            // This is a Unity's bug; issue id is 1253433.
            // The following null check is a work-around for this.
            if (_regexes == null)
            {
                _regexes = new List<Regex>();
            }

            _regexes.Clear();
            foreach (var assetPathRegex in _assetPathRegex)
            {
                if (string.IsNullOrEmpty(assetPathRegex))
                {
                    continue;
                }

                try
                {
                    var regex = new Regex(assetPathRegex);
                    _regexes.Add(regex);
                }
                catch
                {
                    // If the regex string is invalid and an exception is thrown, continue.
                }
            }
        }

        /// <inheritdoc />
        public bool IsMatch(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return false;
            }

            foreach (var regex in _regexes)
            {
                if (regex.IsMatch(assetPath))
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
            foreach (var assetPathRegex in _assetPathRegex)
            {
                if (string.IsNullOrEmpty(assetPathRegex))
                {
                    continue;
                }

                if (!isFirstItem)
                {
                    result.Append(", ");
                }

                result.Append(assetPathRegex);
                isFirstItem = false;
            }

            if (result.Length >= 1)
            {
                result.Insert(0, "Regexes: ");
            }

            return result.ToString();
        }
    }
}

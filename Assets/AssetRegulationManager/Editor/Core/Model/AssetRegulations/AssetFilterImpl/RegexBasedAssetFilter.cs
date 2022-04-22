// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl
{
    /// <summary>
    ///     Filter to pass assets if matches the regex.
    /// </summary>
    [Serializable]
    [AssetFilter("Asset Path Filter", "Asset Path Filter")]
    public sealed class RegexBasedAssetFilter : AssetFilterBase
    {
        [SerializeField] private AssetFilterCondition _condition = AssetFilterCondition.Or;
        [SerializeField] private StringListableProperty _assetPathRegex = new StringListableProperty();
        private List<Regex> _regexes = new List<Regex>();

        public AssetFilterCondition Condition
        {
            get => _condition;
            set => _condition = value;
        }

        /// <summary>
        ///     Regex string.
        /// </summary>
        public StringListableProperty AssetPathRegex => _assetPathRegex;

        public override void SetupForMatching()
        {
            _regexes.Clear();
            foreach (var assetPathRegex in _assetPathRegex)
            {
                if (string.IsNullOrEmpty(assetPathRegex))
                    continue;

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
        public override bool IsMatch(string assetPath, Type assetType)
        {
            if (string.IsNullOrEmpty(assetPath))
                return false;

            switch (_condition)
            {
                case AssetFilterCondition.And:
                    for (var i = 0; i < _regexes.Count; i++)
                        if (!_regexes[i].IsMatch(assetPath))
                            return false;
                    return true;
                case AssetFilterCondition.Or:
                    for (var i = 0; i < _regexes.Count; i++)
                        if (_regexes[i].IsMatch(assetPath))
                            return true;
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string GetDescription()
        {
            var result = new StringBuilder();
            var elementCount = 0;
            foreach (var assetPathRegex in _assetPathRegex)
            {
                if (string.IsNullOrEmpty(assetPathRegex))
                    continue;

                if (elementCount >= 1)
                {
                    var delimiter = _condition == AssetFilterCondition.And ? " && " : " || ";
                    result.Append(delimiter);
                }

                result.Append(assetPathRegex);
                elementCount++;
            }

            if (result.Length >= 1)
            {
                if (elementCount >= 2)
                {
                    result.Insert(0, "( ");
                    result.Append(" )");
                }

                result.Insert(0, "Asset Path: ");
            }

            return result.ToString();
        }
    }
}

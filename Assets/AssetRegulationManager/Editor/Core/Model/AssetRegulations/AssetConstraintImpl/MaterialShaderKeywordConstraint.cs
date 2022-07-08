using System;
using System.Linq;
using System.Text;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEngine;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("Material/Shader Keyword", "Shader Keyword")]
    public sealed class MaterialShaderKeywordConstraint : AssetConstraint<Material>
    {
        public enum CheckCondition
        {
            EnabledAll,
            EnabledAny,
            DisabledAll,
            DisabledAny
        }

        [SerializeField] private CheckCondition _checkCondition = CheckCondition.EnabledAll;
        [SerializeField] private StringListableProperty _keywords = new StringListableProperty();

        private string[] _latestValues;

        public CheckCondition Condition
        {
            get => _checkCondition;
            set => _checkCondition = value;
        }

        public StringListableProperty Keywords => _keywords;

        public override string GetDescription()
        {
            var result = new StringBuilder();
            var elementCount = 0;
            foreach (var keyword in _keywords)
            {
                if (string.IsNullOrEmpty(keyword))
                    continue;

                if (elementCount >= 1)
                {
                    var delimiter =
                        _checkCondition == CheckCondition.EnabledAll || _checkCondition == CheckCondition.DisabledAll
                            ? " && "
                            : " || ";
                    result.Append(delimiter);
                }

                result.Append(keyword);
                elementCount++;
            }

            if (result.Length >= 1)
            {
                if (elementCount >= 2)
                {
                    result.Insert(0, "( ");
                    result.Append(" )");
                }

                var prefix = _checkCondition == CheckCondition.EnabledAll ||
                             _checkCondition == CheckCondition.EnabledAny
                    ? "Enabled Shader Keyword: "
                    : "Disabled Shader Keyword: ";
                result.Insert(0, prefix);
            }

            return result.ToString();
        }

        public override string GetLatestValueAsText()
        {
            return string.Join(", ", _latestValues);
        }

        protected override bool CheckInternal(Material asset)
        {
            Assert.IsNotNull(asset);

            _latestValues = asset.shaderKeywords;
            switch (_checkCondition)
            {
                case CheckCondition.EnabledAny:
                    return _keywords.Any(keyword => asset.shaderKeywords.Contains(keyword));
                case CheckCondition.EnabledAll:
                    return _keywords.All(keyword => asset.shaderKeywords.Contains(keyword));
                case CheckCondition.DisabledAny:
                    return _keywords.Any(keyword => !asset.shaderKeywords.Contains(keyword));
                case CheckCondition.DisabledAll:
                    return _keywords.All(keyword => !asset.shaderKeywords.Contains(keyword));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

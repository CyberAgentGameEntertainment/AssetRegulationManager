using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class MaterialShaderKeywordConstraintTest
    {
        private const string MetallicMapKeyword = "_METALLICGLOSSMAP";
        private const string ParallaxMapKeyword = "_PARALLAXMAP";
        private const string InvalidKeyword = "_INVALID_KEYWORD";
        private const string InvalidKeyword2 = "_INVALID_KEYWORD_2";
        
        [TestCase(MetallicMapKeyword, ExpectedResult = true)]
        [TestCase(InvalidKeyword, ExpectedResult = false)]
        public bool Check(string keyword)
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>(TestAssetPaths.MaterialTex64MetallicParallelMap);
            var constraint = new MaterialShaderKeywordConstraint();
            constraint.Condition = MaterialShaderKeywordConstraint.CheckCondition.EnabledAll;
            constraint.Keywords.Value = keyword;
            return constraint.Check(material);
        }
        
        [TestCase(MetallicMapKeyword, ParallaxMapKeyword, MaterialShaderKeywordConstraint.CheckCondition.EnabledAll, ExpectedResult = true)]
        [TestCase(MetallicMapKeyword, InvalidKeyword, MaterialShaderKeywordConstraint.CheckCondition.EnabledAll, ExpectedResult = false)]
        [TestCase(MetallicMapKeyword, InvalidKeyword, MaterialShaderKeywordConstraint.CheckCondition.EnabledAny, ExpectedResult = true)]
        [TestCase(InvalidKeyword, InvalidKeyword2, MaterialShaderKeywordConstraint.CheckCondition.EnabledAny, ExpectedResult = false)]
        [TestCase(InvalidKeyword, InvalidKeyword2, MaterialShaderKeywordConstraint.CheckCondition.DisabledAll, ExpectedResult = true)]
        [TestCase(MetallicMapKeyword, InvalidKeyword, MaterialShaderKeywordConstraint.CheckCondition.DisabledAll, ExpectedResult = false)]
        [TestCase(MetallicMapKeyword, InvalidKeyword, MaterialShaderKeywordConstraint.CheckCondition.DisabledAny, ExpectedResult = true)]
        [TestCase(MetallicMapKeyword, ParallaxMapKeyword, MaterialShaderKeywordConstraint.CheckCondition.DisabledAny, ExpectedResult = false)]
        public bool Check_Condition(string keyword1, string keyword2, MaterialShaderKeywordConstraint.CheckCondition condition)
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>(TestAssetPaths.MaterialTex64MetallicParallelMap);
            var constraint = new MaterialShaderKeywordConstraint();
            constraint.Condition = condition;
            constraint.Keywords.IsListMode = true;
            constraint.Keywords.AddValue(keyword1);
            constraint.Keywords.AddValue(keyword2);
            return constraint.Check(material);
        }
    }
}

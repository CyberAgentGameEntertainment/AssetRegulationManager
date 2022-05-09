// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl;
using NUnit.Framework;

namespace AssetRegulationManager.Tests.Editor.AssetFilterImpl
{
    internal sealed class RegexBasedAssetFilterTest
    {
        [Test]
        public void IsMatch_RegisterMatchedRegex_ReturnTrue()
        {
            var filter = new RegexBasedAssetFilter();
            filter.AssetPathRegex.Value = "^Assets/Test/.+";
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test", null), Is.True);
        }

        [Test]
        public void IsMatch_RegisterNotMatchedRegex_ReturnFalse()
        {
            var filter = new RegexBasedAssetFilter();
            filter.AssetPathRegex.Value = "^Assets/Test2/.+";
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test", null), Is.False);
        }

        [Test]
        public void IsMatch_RegisterInvalidRegex_ReturnFalse()
        {
            var filter = new RegexBasedAssetFilter();
            filter.AssetPathRegex.Value = "^Assets/(Test/.+";
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test", null), Is.False);
        }

        [Test]
        public void IsMatch_MatchAnyCondition_ContainsMatchedValue_ReturnTrue()
        {
            var filter = new RegexBasedAssetFilter();
            filter.Condition = AssetFilterCondition.MatchAny;
            filter.AssetPathRegex.IsListMode = true;
            filter.AssetPathRegex.AddValue("^Assets/Test/.+");
            filter.AssetPathRegex.AddValue("^Assets/Test2/.+");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test", null), Is.True);
        }

        [Test]
        public void IsMatch_MatchAnyCondition_AllValuesNotMatch_ReturnFalse()
        {
            var filter = new RegexBasedAssetFilter();
            filter.Condition = AssetFilterCondition.MatchAny;
            filter.AssetPathRegex.IsListMode = true;
            filter.AssetPathRegex.AddValue("^Assets/Test2/.+");
            filter.AssetPathRegex.AddValue("^Assets/Test3/.+");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test", null), Is.False);
        }

        [Test]
        public void IsMatch_MatchAllCondition_AllValuesMatch_ReturnTrue()
        {
            var filter = new RegexBasedAssetFilter();
            filter.Condition = AssetFilterCondition.MatchAll;
            filter.AssetPathRegex.IsListMode = true;
            filter.AssetPathRegex.AddValue("^Assets/Test/.+");
            filter.AssetPathRegex.AddValue(".+/Test/.+");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test", null), Is.True);
        }

        [Test]
        public void IsMatch_MatchAllCondition_ContainsUnmatched_ReturnFalse()
        {
            var filter = new RegexBasedAssetFilter();
            filter.Condition = AssetFilterCondition.MatchAll;
            filter.AssetPathRegex.IsListMode = true;
            filter.AssetPathRegex.AddValue("^Assets/Test/.+");
            filter.AssetPathRegex.AddValue(".+/NotMatched/.+");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test", null), Is.False);
        }

        [Test]
        public void IsMatch_NotMatchAnyCondition_ContainsUnmatched_ReturnTrue()
        {
            var filter = new RegexBasedAssetFilter();
            filter.Condition = AssetFilterCondition.NotMatchAny;
            filter.AssetPathRegex.IsListMode = true;
            filter.AssetPathRegex.AddValue("^Assets/Test/.+");
            filter.AssetPathRegex.AddValue("^Assets/Test2/.+");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test", null), Is.True);
        }

        [Test]
        public void IsMatch_NotMatchAnyCondition_AllValuesMatch_ReturnFalse()
        {
            var filter = new RegexBasedAssetFilter();
            filter.Condition = AssetFilterCondition.NotMatchAny;
            filter.AssetPathRegex.IsListMode = true;
            filter.AssetPathRegex.AddValue("^Assets/Test/.+");
            filter.AssetPathRegex.AddValue("^Assets/Test/Test.+");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test", null), Is.False);
        }

        [Test]
        public void IsMatch_NotMatchAllCondition_AllValuesNotMatch_ReturnTrue()
        {
            var filter = new RegexBasedAssetFilter();
            filter.Condition = AssetFilterCondition.NotMatchAll;
            filter.AssetPathRegex.IsListMode = true;
            filter.AssetPathRegex.AddValue("^Assets/Test2/.+");
            filter.AssetPathRegex.AddValue(".+/Test2/.+");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test", null), Is.True);
        }

        [Test]
        public void IsMatch_NotMatchAllCondition_ContainsMatched_ReturnFalse()
        {
            var filter = new RegexBasedAssetFilter();
            filter.Condition = AssetFilterCondition.NotMatchAll;
            filter.AssetPathRegex.IsListMode = true;
            filter.AssetPathRegex.AddValue("^Assets/Test/.+");
            filter.AssetPathRegex.AddValue(".+/NotMatched/.+");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test", null), Is.False);
        }
    }
}

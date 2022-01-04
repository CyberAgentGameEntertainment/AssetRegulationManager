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
            Assert.That(filter.IsMatch("Assets/Test/Test.test"), Is.True);
        }

        [Test]
        public void IsMatch_RegisterNotMatchedRegex_ReturnFalse()
        {
            var filter = new RegexBasedAssetFilter();
            filter.AssetPathRegex.Value = "^Assets/Test2/.+";
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test"), Is.False);
        }

        [Test]
        public void IsMatch_RegisterInvalidRegex_ReturnFalse()
        {
            var filter = new RegexBasedAssetFilter();
            filter.AssetPathRegex.Value = "^Assets/(Test/.+";
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test"), Is.False);
        }

        [Test]
        public void IsMatch_RegisterRegexesAndContainsMatched_ReturnTrue()
        {
            var filter = new RegexBasedAssetFilter();
            filter.AssetPathRegex.IsListMode = true;
            filter.AssetPathRegex.AddValue("^Assets/Test/.+");
            filter.AssetPathRegex.AddValue("^Assets/Test2/.+");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test"), Is.True);
        }

        [Test]
        public void IsMatch_RegisterExtensionsAndNotContainsMatched_ReturnFalse()
        {
            var filter = new RegexBasedAssetFilter();
            filter.AssetPathRegex.IsListMode = true;
            filter.AssetPathRegex.AddValue("^Assets/Test2/.+");
            filter.AssetPathRegex.AddValue("^Assets/Test3/.+");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Assets/Test/Test.test"), Is.False);
        }
    }
}

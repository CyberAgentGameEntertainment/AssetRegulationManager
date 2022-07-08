// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl;
using NUnit.Framework;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetFilterImpl
{
    internal sealed class ExtensionBasedAssetFilterTest
    {
        [Test]
        public void IsMatch_RegisterMatchedExtension_ReturnTrue()
        {
            var filter = new ExtensionBasedAssetFilter();
            filter.Extension.Value = "png";
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Test.png", typeof(Texture2D), false), Is.True);
        }

        [Test]
        public void IsMatch_RegisterMatchedExtensionWithDot_ReturnTrue()
        {
            var filter = new ExtensionBasedAssetFilter();
            filter.Extension.Value = ".png";
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Test.png", typeof(Texture2D), false), Is.True);
        }

        [Test]
        public void IsMatch_RegisterNotMatchedExtension_ReturnFalse()
        {
            var filter = new ExtensionBasedAssetFilter();
            filter.Extension.Value = "jpg";
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Test.png", typeof(Texture2D), false), Is.False);
        }

        [Test]
        public void IsMatch_RegisterExtensionsAndContainsMatched_ReturnTrue()
        {
            var filter = new ExtensionBasedAssetFilter();
            filter.Extension.IsListMode = true;
            filter.Extension.AddValue("png");
            filter.Extension.AddValue("jpg");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Test.png", typeof(Texture2D), false), Is.True);
        }

        [Test]
        public void IsMatch_RegisterExtensionsAndNotContainsMatched_ReturnFalse()
        {
            var filter = new ExtensionBasedAssetFilter();
            filter.Extension.IsListMode = true;
            filter.Extension.AddValue("jpg");
            filter.Extension.AddValue("exr");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Test.png", typeof(Texture2D), false), Is.False);
        }
    }
}

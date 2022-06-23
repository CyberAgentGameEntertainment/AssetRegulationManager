// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Tests.Editor.AssetFilterImpl
{
    internal sealed class ObjectBasedAssetFilterTest
    {
        [Test]
        public void IsMatch_RegisterMatchedObject_ReturnTrue()
        {
            var filter = new ObjectBasedAssetFilter();
            filter.Object.Value = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64);
            filter.SetupForMatching();
            Assert.That(filter.IsMatch(TestAssetPaths.Texture64, typeof(Texture2D), false), Is.True);
        }

        [TestCase(FolderTargetingMode.IncludedNonFolderAssets, TestAssetPaths.Texture64, typeof(Texture2D),
            ExpectedResult = true)]
        [TestCase(FolderTargetingMode.IncludedNonFolderAssets, TestAssetPaths.BaseFolderPath, typeof(DefaultAsset),
            ExpectedResult = false)]
        [TestCase(FolderTargetingMode.Self, TestAssetPaths.Texture64, typeof(Texture2D), ExpectedResult = false)]
        [TestCase(FolderTargetingMode.Self, TestAssetPaths.BaseFolderPath, typeof(DefaultAsset), ExpectedResult = true)]
        [TestCase(FolderTargetingMode.Both, TestAssetPaths.Texture64, typeof(Texture2D), ExpectedResult = true)]
        [TestCase(FolderTargetingMode.Both, TestAssetPaths.BaseFolderPath, typeof(DefaultAsset), ExpectedResult = true)]
        public bool IsMatch_ObjectIsFolder(FolderTargetingMode targetingMode, string assetPath, Type assetType)
        {
            var filter = new ObjectBasedAssetFilter();
            filter.FolderTargetingMode = targetingMode;
            filter.Object.Value = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.BaseFolderPath);
            filter.SetupForMatching();
            return filter.IsMatch(assetPath, assetType, assetType == typeof(DefaultAsset));
        }

        [Test]
        public void IsMatch_RegisterNotMatchedObject_ReturnFalse()
        {
            var filter = new ObjectBasedAssetFilter();
            filter.Object.Value = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64);
            filter.SetupForMatching();
            Assert.That(filter.IsMatch(TestAssetPaths.Texture128, typeof(Texture2D), false), Is.False);
        }

        [Test]
        public void IsMatch_RegisterObjectsAndContainsMatched_ReturnTrue()
        {
            var filter = new ObjectBasedAssetFilter();
            filter.Object.IsListMode = true;
            filter.Object.AddValue(AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64));
            filter.Object.AddValue(AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture128));
            filter.SetupForMatching();
            Assert.That(filter.IsMatch(TestAssetPaths.Texture64, typeof(Texture2D), false), Is.True);
        }

        [Test]
        public void IsMatch_RegisterExtensionsAndNotContainsMatched_ReturnFalse()
        {
            var filter = new ObjectBasedAssetFilter();
            filter.Object.IsListMode = true;
            filter.Object.AddValue(AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture128));
            filter.Object.AddValue(AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture128MaxSize64));
            filter.SetupForMatching();
            Assert.That(filter.IsMatch(TestAssetPaths.Texture64, typeof(Texture2D), false), Is.False);
        }
    }
}

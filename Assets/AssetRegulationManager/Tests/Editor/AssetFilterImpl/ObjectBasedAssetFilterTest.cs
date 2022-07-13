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

        [TestCase(FolderTargetingMode.IncludedNonFolderAssets, TestAssetRelativePaths.Texture64, typeof(Texture2D),
            ExpectedResult = true)]
        [TestCase(FolderTargetingMode.IncludedNonFolderAssets, "", typeof(DefaultAsset), ExpectedResult = false)]
        [TestCase(FolderTargetingMode.Self, TestAssetRelativePaths.Texture64, typeof(Texture2D),
            ExpectedResult = false)]
        [TestCase(FolderTargetingMode.Self, "", typeof(DefaultAsset), ExpectedResult = true)]
        [TestCase(FolderTargetingMode.Both, TestAssetRelativePaths.Texture64, typeof(Texture2D), ExpectedResult = true)]
        [TestCase(FolderTargetingMode.Both, "", typeof(DefaultAsset), ExpectedResult = true)]
        public bool IsMatch_ObjectIsFolder(FolderTargetingMode targetingMode, string assetRelativePath, Type assetType)
        {
            var assetPath = TestAssetPaths.CreateAbsoluteAssetPath(assetRelativePath);
            var filter = new ObjectBasedAssetFilter();
            filter.FolderTargetingMode = targetingMode;
            filter.Object.Value = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Folder);
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

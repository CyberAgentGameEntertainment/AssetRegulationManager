using System;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Tests.Editor.AssetFilterImpl
{
    internal sealed class DependentObjectBasedAssetFilterTest
    {
        [TestCase(TestAssetRelativePaths.Texture64, typeof(Texture2D), ExpectedResult = true)]
        [TestCase(TestAssetRelativePaths.Texture2048, typeof(Texture2D), ExpectedResult = false)]
        [TestCase(TestAssetRelativePaths.PrefabTexel64x2And128, typeof(GameObject), ExpectedResult = true)]
        public bool IsMatch(string assetRelativePath, Type assetType)
        {
            var assetPath = TestAssetPaths.CreateAbsoluteAssetPath(assetRelativePath);
            var filter = new DependentObjectBasedAssetFilter();
            filter.Object.Value = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.PrefabTexel64x2And128);
            filter.SetupForMatching();
            return filter.IsMatch(assetPath, assetType, assetType == typeof(DefaultAsset));
        }

        [TestCase(TestAssetRelativePaths.Texture64, typeof(Texture2D), false, ExpectedResult = true)]
        [TestCase(TestAssetRelativePaths.Texture64, typeof(Texture2D), true, ExpectedResult = false)]
        [TestCase(TestAssetRelativePaths.MaterialTex64, typeof(Material), false, ExpectedResult = true)]
        [TestCase(TestAssetRelativePaths.MaterialTex64, typeof(Material), true, ExpectedResult = true)]
        public bool IsMatch_OnlyDirectDependencies(string assetRelativePath, Type assetType, bool onlyDirectDependencies)
        {
            var assetPath = TestAssetPaths.CreateAbsoluteAssetPath(assetRelativePath);
            var filter = new DependentObjectBasedAssetFilter();
            filter.Object.Value = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.PrefabTexel64x2And128);
            filter.OnlyDirectDependencies = onlyDirectDependencies;
            filter.SetupForMatching();
            return filter.IsMatch(assetPath, assetType, assetType == typeof(DefaultAsset));
        }
    }
}

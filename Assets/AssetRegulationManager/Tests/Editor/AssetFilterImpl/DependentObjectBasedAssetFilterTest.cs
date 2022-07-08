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
        [TestCase(TestAssetPaths.Texture64, typeof(Texture2D), ExpectedResult = true)]
        [TestCase(TestAssetPaths.Texture2048, typeof(Texture2D), ExpectedResult = false)]
        [TestCase(TestAssetPaths.PrefabTexel64x2And128, typeof(GameObject), ExpectedResult = true)]
        public bool IsMatch(string assetPath, Type assetType)
        {
            var filter = new DependentObjectBasedAssetFilter();
            filter.Object.Value = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.PrefabTexel64x2And128);
            filter.SetupForMatching();
            return filter.IsMatch(assetPath, assetType, assetType == typeof(DefaultAsset));
        }

        [TestCase(TestAssetPaths.Texture64, typeof(Texture2D), false, ExpectedResult = true)]
        [TestCase(TestAssetPaths.Texture64, typeof(Texture2D), true, ExpectedResult = false)]
        [TestCase(TestAssetPaths.MaterialTex64, typeof(Material), false, ExpectedResult = true)]
        [TestCase(TestAssetPaths.MaterialTex64, typeof(Material), true, ExpectedResult = true)]
        public bool IsMatch_OnlyDirectDependencies(string assetPath, Type assetType, bool onlyDirectDependencies)
        {
            var filter = new DependentObjectBasedAssetFilter();
            filter.Object.Value = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.PrefabTexel64x2And128);
            filter.OnlyDirectDependencies = onlyDirectDependencies;
            filter.SetupForMatching();
            return filter.IsMatch(assetPath, assetType, assetType == typeof(DefaultAsset));
        }
    }
}

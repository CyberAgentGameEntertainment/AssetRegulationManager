using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;

namespace AssetRegulationManager.Tests.Editor
{
    internal sealed class AssetPathTypeTest
    {
        [Test]
        public void ConvertAssetPath_AssetPath_ReturnsAssetPath()
        {
            var assetPath = "Assets/DummyAsset.asset";
            var result = AssetPathType.AssetPath.ConvertAssetPath(assetPath);
            Assert.That(result, Is.EqualTo("Assets/DummyAsset.asset"));
        }

        [Test]
        public void ConvertAssetPath_WindowAssetPath_ReturnsAssetPath()
        {
            var assetPath = "Assets\\DummyAsset.asset";
            var result = AssetPathType.AssetPath.ConvertAssetPath(assetPath);
            Assert.That(result, Is.EqualTo("Assets/DummyAsset.asset"));
        }

        [Test]
        public void ConvertAssetPath_AssetName_ReturnsAssetName()
        {
            var assetPath = "Assets/DummyAsset.asset";
            var result = AssetPathType.AssetName.ConvertAssetPath(assetPath);
            Assert.That(result, Is.EqualTo("DummyAsset.asset"));
        }
        
        [Test]
        public void ConvertAssetPath_AssetNameWithoutExtensions_ReturnsAssetNameWithoutExtensions()
        {
            var assetPath = "Assets/DummyAsset.asset";
            var result = AssetPathType.AssetNameWithoutExtensions.ConvertAssetPath(assetPath);
            Assert.That(result, Is.EqualTo("DummyAsset"));
        }

        [Test]
        public void ConvertAssetPath_FolderPath_ReturnsFolderPath()
        {
            var assetPath = "Assets/Dummy/DummyAsset.asset";
            var result = AssetPathType.FolderPath.ConvertAssetPath(assetPath);
            Assert.That(result, Is.EqualTo("Assets/Dummy"));
        }

        [Test]
        public void ConvertAssetPath_WindowsFolderPath_ReturnsFolderPath()
        {
            var assetPath = "Assets\\Dummy\\DummyAsset.asset";
            var result = AssetPathType.FolderPath.ConvertAssetPath(assetPath);
            Assert.That(result, Is.EqualTo("Assets/Dummy"));
        }
        
        [Test]
        public void ConvertAssetPath_FolderName_ReturnsFolderName()
        {
            var assetPath = "Assets/Dummy/DummyAsset.asset";
            var result = AssetPathType.FolderName.ConvertAssetPath(assetPath);
            Assert.That(result, Is.EqualTo("Dummy"));
        }
    }
}

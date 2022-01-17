using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl;
using NUnit.Framework;
using UnityEditor;

namespace AssetRegulationManager.Tests.Editor.AssetLimitationImpl
{
    internal sealed class MaxSceneVertexCountLimitationTest
    {
        [Test]
        public void Check_CountIsEqualsToLimitation_ReturnTrue()
        {
            var limitation = new MaxSceneVertexCountLimitation();
            limitation.MaxCount = 24;
            limitation.AllowDuplicateCount = false;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene24x3Vertices);
            Assert.That(limitation.Check(obj), Is.True);
        }

        [Test]
        public void Check_CountIsGreaterThanLimitation_ReturnFalse()
        {
            var limitation = new MaxSceneVertexCountLimitation();
            limitation.MaxCount = 23;
            limitation.AllowDuplicateCount = false;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene24x3Vertices);
            Assert.That(limitation.Check(obj), Is.False);
        }

        [Test]
        public void Check_AllowDuplicateAndCountIsEqualsToLimitation_ReturnTrue()
        {
            var limitation = new MaxSceneVertexCountLimitation();
            limitation.MaxCount = 72;
            limitation.AllowDuplicateCount = true;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene24x3Vertices);
            Assert.That(limitation.Check(obj), Is.True);
        }

        [Test]
        public void Check_AllowDuplicateAndCountIsGreaterThanLimitation_ReturnFalse()
        {
            var limitation = new MaxSceneVertexCountLimitation();
            limitation.MaxCount = 71;
            limitation.AllowDuplicateCount = true;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene24x3Vertices);
            Assert.That(limitation.Check(obj), Is.False);
        }
    }
}

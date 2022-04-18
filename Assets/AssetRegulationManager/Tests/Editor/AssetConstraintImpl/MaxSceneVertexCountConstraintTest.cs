using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class MaxSceneVertexCountConstraintTest
    {
        [Test]
        public void Check_CountIsEqualsToConstraint_ReturnTrue()
        {
            var constraint = new MaxSceneVertexCountConstraint();
            constraint.MaxCount = 24;
            constraint.AllowDuplicateCount = false;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene24x3Vertices);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_CountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxSceneVertexCountConstraint();
            constraint.MaxCount = 23;
            constraint.AllowDuplicateCount = false;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene24x3Vertices);
            Assert.That(constraint.Check(obj), Is.False);
        }

        [Test]
        public void Check_AllowDuplicateAndCountIsEqualsToConstraint_ReturnTrue()
        {
            var constraint = new MaxSceneVertexCountConstraint();
            constraint.MaxCount = 72;
            constraint.AllowDuplicateCount = true;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene24x3Vertices);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_AllowDuplicateAndCountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxSceneVertexCountConstraint();
            constraint.MaxCount = 71;
            constraint.AllowDuplicateCount = true;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene24x3Vertices);
            Assert.That(constraint.Check(obj), Is.False);
        }
    }
}

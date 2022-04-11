using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class MaxSceneTexelCountConstraintTest
    {
        [Test]
        public static void Check_CountIsEqualsToConstraint_ReturnTrue()
        {
            var constraint = new MaxSceneTexelCountConstraint();
            constraint.ExcludeInactive = false;
            constraint.AllowDuplicateCount = false;
            constraint.MaxCount = 64 * 64 + 128 * 128;
            var asset = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.SceneTexel64x2And128);

            Assert.That(constraint.Check(asset), Is.True);
        }

        [Test]
        public static void Check_CountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxSceneTexelCountConstraint();
            constraint.ExcludeInactive = false;
            constraint.AllowDuplicateCount = false;
            constraint.MaxCount = 64 * 64 + 128 * 128 - 1;
            var asset = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.SceneTexel64x2And128);

            Assert.That(constraint.Check(asset), Is.False);
        }

        [Test]
        public static void Check_AllowDuplicateCountAndCountIsEqualsToConstraint_ReturnTrue()
        {
            var constraint = new MaxSceneTexelCountConstraint();
            constraint.ExcludeInactive = false;
            constraint.AllowDuplicateCount = true;
            constraint.MaxCount = 64 * 64 * 2 + 128 * 128;
            var asset = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.SceneTexel64x2And128);

            Assert.That(constraint.Check(asset), Is.True);
        }

        [Test]
        public static void Check_AllowDuplicateCountAndCountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxSceneTexelCountConstraint();
            constraint.ExcludeInactive = false;
            constraint.AllowDuplicateCount = true;
            constraint.MaxCount = 64 * 64 * 2 + 128 * 128 - 1;
            var asset = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.SceneTexel64x2And128);

            Assert.That(constraint.Check(asset), Is.False);
        }
    }
}

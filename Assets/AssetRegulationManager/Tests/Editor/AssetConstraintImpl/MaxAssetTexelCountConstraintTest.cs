using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class MaxAssetTexelCountConstraintTest
    {
        [Test]
        public static void Check_CountIsEqualsToConstraint_ReturnTrue()
        {
            var constraint = new MaxAssetTexelCountConstraint();
            constraint.MaxCount = 64 * 64;
            var asset = AssetDatabase.LoadAssetAtPath<Material>(TestAssetPaths.MaterialTex64Duplicated);

            Assert.That(constraint.Check(asset), Is.True);
        }

        [Test]
        public static void Check_CountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxAssetTexelCountConstraint();
            constraint.MaxCount = 64 * 64 - 1;
            var asset = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.MaterialTex64Duplicated);

            Assert.That(constraint.Check(asset), Is.False);
        }
    }
}

// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class MaxParticleSystemCountConstraintTest
    {
        [Test]
        public void Check_CountIsEqualsToConstraint_ReturnTrue()
        {
            var constraint = new MaxParticleSystemCountConstraint();
            constraint.MaxCount = 3;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.Prefab3Particles);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_CountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxParticleSystemCountConstraint();
            constraint.MaxCount = 2;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.Prefab3Particles);
            Assert.That(constraint.Check(obj), Is.False);
        }
    }
}

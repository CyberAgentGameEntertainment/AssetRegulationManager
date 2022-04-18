// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class MaxSceneParticleSystemCountLimitationTest
    {
        [Test]
        public void Check_CountIsEqualsToLimitation_ReturnTrue()
        {
            var limitation = new MaxSceneParticleSystemCountConstraint();
            limitation.MaxCount = 3;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene3Particles);
            Assert.That(limitation.Check(obj), Is.True);
        }

        [Test]
        public void Check_CountIsGreaterThanLimitation_ReturnFalse()
        {
            var limitation = new MaxSceneParticleSystemCountConstraint();
            limitation.MaxCount = 2;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene3Particles);
            Assert.That(limitation.Check(obj), Is.False);
        }
    }
}

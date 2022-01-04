// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetLimitationImpl
{
    internal sealed class MaxParticleSystemCountLimitationTest
    {
        [Test]
        public void Check_CountIsEqualsToLimitation_ReturnTrue()
        {
            var limitation = new MaxParticleSystemCountLimitation();
            limitation.MaxCount = 3;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.Prefab3Particles);
            Assert.That(limitation.Check(obj), Is.True);
        }

        [Test]
        public void Check_CountIsGreaterThanLimitation_ReturnFalse()
        {
            var limitation = new MaxParticleSystemCountLimitation();
            limitation.MaxCount = 2;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.Prefab3Particles);
            Assert.That(limitation.Check(obj), Is.False);
        }
    }
}

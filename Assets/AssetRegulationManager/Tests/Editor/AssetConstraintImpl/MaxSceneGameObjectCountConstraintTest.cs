// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class MaxSceneGameObjectCountConstraintTest
    {
        [Test]
        public void Check_GameObjectCountIsEqualsToConstraint_ReturnTrue()
        {
            var constraint = new MaxSceneGameObjectCountConstraint();
            constraint.MaxCount = 3;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene3Obj);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_FbxVertexCountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxSceneGameObjectCountConstraint();
            constraint.MaxCount = 2;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene3Obj);
            Assert.That(constraint.Check(obj), Is.False);
        }
    }
}

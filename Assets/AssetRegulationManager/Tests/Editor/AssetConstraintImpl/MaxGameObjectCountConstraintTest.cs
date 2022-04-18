// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class MaxGameObjectCountConstraintTest
    {
        [Test]
        public void Check_GameObjectCountIsEqualsToConstraint_ReturnTrue()
        {
            var constraint = new MaxGameObjectCountConstraint();
            constraint.MaxCount = 3;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.Prefab3Obj);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_FbxVertexCountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxGameObjectCountConstraint();
            constraint.MaxCount = 2;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.Prefab3Obj);
            Assert.That(constraint.Check(obj), Is.False);
        }
    }
}

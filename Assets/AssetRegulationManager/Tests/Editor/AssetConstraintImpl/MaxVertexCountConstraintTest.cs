// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class MaxVertexCountConstraintTest
    {
        [Test]
        public void Check_FbxVertexCountIsEqualsToConstraint_ReturnTrue()
        {
            var constraint = new MaxVertexCountConstraint();
            constraint.MaxCount = 24;
            constraint.ExcludeChildren = true;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.FbxSingleMesh);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_FbxVertexCountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxVertexCountConstraint();
            constraint.MaxCount = 23;
            constraint.ExcludeChildren = true;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.FbxSingleMesh);
            Assert.That(constraint.Check(obj), Is.False);
        }

        [Test]
        public void Check_TotalFbxVertexCountIsEqualToConstraint_ReturnTrue()
        {
            var constraint = new MaxVertexCountConstraint();
            constraint.MaxCount = 72;
            constraint.ExcludeChildren = false;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.FbxMultiMesh);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_TotalFbxVertexCountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxVertexCountConstraint();
            constraint.MaxCount = 71;
            constraint.ExcludeChildren = false;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.FbxMultiMesh);
            Assert.That(constraint.Check(obj), Is.False);
        }

        [Test]
        public void Check_PrefabVertexCountIsEqualsToConstraint_ReturnTrue()
        {
            var constraint = new MaxVertexCountConstraint();
            constraint.MaxCount = 24;
            constraint.ExcludeChildren = true;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.PrefabSingleMesh);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_PrefabVertexCountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxVertexCountConstraint();
            constraint.MaxCount = 23;
            constraint.ExcludeChildren = true;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.PrefabSingleMesh);
            Assert.That(constraint.Check(obj), Is.False);
        }

        [Test]
        public void Check_TotalPrefabVertexCountIsEqualToConstraint_ReturnTrue()
        {
            var constraint = new MaxVertexCountConstraint();
            constraint.MaxCount = 24;
            constraint.ExcludeChildren = false;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.PrefabMultiMesh);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_TotalPrefabVertexCountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxVertexCountConstraint();
            constraint.MaxCount = 23;
            constraint.ExcludeChildren = false;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.PrefabMultiMesh);
            Assert.That(constraint.Check(obj), Is.False);
        }

        [Test]
        public void Check_AllowDuplicateCountAndTotalPrefabVertexCountIsEqualToConstraint_ReturnTrue()
        {
            var constraint = new MaxVertexCountConstraint();
            constraint.MaxCount = 72;
            constraint.ExcludeChildren = false;
            constraint.AllowDuplicateCount = true;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.PrefabMultiMesh);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_AllowDuplicateCountAndTotalPrefabVertexCountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxVertexCountConstraint();
            constraint.MaxCount = 71;
            constraint.ExcludeChildren = false;
            constraint.AllowDuplicateCount = true;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.PrefabMultiMesh);
            Assert.That(constraint.Check(obj), Is.False);
        }

        [Test]
        public void Check_MeshVertexCountIsEqualsToConstraint_ReturnTrue()
        {
            var constraint = new MaxVertexCountConstraint();
            constraint.MaxCount = 24;
            constraint.ExcludeChildren = true;
            var obj = AssetDatabase.LoadAssetAtPath<Mesh>(TestAssetPaths.Mesh24verts);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_MeshVertexCountIsGreaterThanConstraint_ReturnFalse()
        {
            var constraint = new MaxVertexCountConstraint();
            constraint.MaxCount = 23;
            constraint.ExcludeChildren = true;
            var obj = AssetDatabase.LoadAssetAtPath<Mesh>(TestAssetPaths.Mesh24verts);
            Assert.That(constraint.Check(obj), Is.False);
        }
    }
}

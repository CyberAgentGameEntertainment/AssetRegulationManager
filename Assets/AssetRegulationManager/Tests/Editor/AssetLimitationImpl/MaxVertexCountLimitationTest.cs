// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetLimitationImpl
{
    internal sealed class MaxVertexCountLimitationTest
    {
        [Test]
        public void Check_FbxVertexCountIsEqualsToLimitation_ReturnTrue()
        {
            var limitation = new MaxVertexCountLimitation();
            limitation.MaxCount = 24;
            limitation.ExcludeChildren = true;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.FbxSingleMesh);
            Assert.That(limitation.Check(obj), Is.True);
        }

        [Test]
        public void Check_FbxVertexCountIsGreaterThanLimitation_ReturnFalse()
        {
            var limitation = new MaxVertexCountLimitation();
            limitation.MaxCount = 23;
            limitation.ExcludeChildren = true;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.FbxSingleMesh);
            Assert.That(limitation.Check(obj), Is.False);
        }

        [Test]
        public void Check_TotalFbxVertexCountIsEqualToLimitation_ReturnTrue()
        {
            var limitation = new MaxVertexCountLimitation();
            limitation.MaxCount = 72;
            limitation.ExcludeChildren = false;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.FbxMultiMesh);
            Assert.That(limitation.Check(obj), Is.True);
        }

        [Test]
        public void Check_TotalFbxVertexCountIsGreaterThanLimitation_ReturnFalse()
        {
            var limitation = new MaxVertexCountLimitation();
            limitation.MaxCount = 71;
            limitation.ExcludeChildren = false;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.FbxMultiMesh);
            Assert.That(limitation.Check(obj), Is.False);
        }

        [Test]
        public void Check_PrefabVertexCountIsEqualsToLimitation_ReturnTrue()
        {
            var limitation = new MaxVertexCountLimitation();
            limitation.MaxCount = 24;
            limitation.ExcludeChildren = true;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.PrefabSingleMesh);
            Assert.That(limitation.Check(obj), Is.True);
        }

        [Test]
        public void Check_PrefabVertexCountIsGreaterThanLimitation_ReturnFalse()
        {
            var limitation = new MaxVertexCountLimitation();
            limitation.MaxCount = 23;
            limitation.ExcludeChildren = true;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.PrefabSingleMesh);
            Assert.That(limitation.Check(obj), Is.False);
        }

        [Test]
        public void Check_TotalPrefabVertexCountIsEqualToLimitation_ReturnTrue()
        {
            var limitation = new MaxVertexCountLimitation();
            limitation.MaxCount = 24;
            limitation.ExcludeChildren = false;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.PrefabMultiMesh);
            Assert.That(limitation.Check(obj), Is.True);
        }

        [Test]
        public void Check_TotalPrefabVertexCountIsGreaterThanLimitation_ReturnFalse()
        {
            var limitation = new MaxVertexCountLimitation();
            limitation.MaxCount = 23;
            limitation.ExcludeChildren = false;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.PrefabMultiMesh);
            Assert.That(limitation.Check(obj), Is.False);
        }

        [Test]
        public void Check_AllowDuplicateCountAndTotalPrefabVertexCountIsEqualToLimitation_ReturnTrue()
        {
            var limitation = new MaxVertexCountLimitation();
            limitation.MaxCount = 72;
            limitation.ExcludeChildren = false;
            limitation.AllowDuplicateCount = true;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.PrefabMultiMesh);
            Assert.That(limitation.Check(obj), Is.True);
        }

        [Test]
        public void Check_AllowDuplicateCountAndTotalPrefabVertexCountIsGreaterThanLimitation_ReturnFalse()
        {
            var limitation = new MaxVertexCountLimitation();
            limitation.MaxCount = 71;
            limitation.ExcludeChildren = false;
            limitation.AllowDuplicateCount = true;
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(TestAssetPaths.PrefabMultiMesh);
            Assert.That(limitation.Check(obj), Is.False);
        }

        [Test]
        public void Check_MeshVertexCountIsEqualsToLimitation_ReturnTrue()
        {
            var limitation = new MaxVertexCountLimitation();
            limitation.MaxCount = 24;
            limitation.ExcludeChildren = true;
            var obj = AssetDatabase.LoadAssetAtPath<Mesh>(TestAssetPaths.Mesh24verts);
            Assert.That(limitation.Check(obj), Is.True);
        }

        [Test]
        public void Check_MeshVertexCountIsGreaterThanLimitation_ReturnFalse()
        {
            var limitation = new MaxVertexCountLimitation();
            limitation.MaxCount = 23;
            limitation.ExcludeChildren = true;
            var obj = AssetDatabase.LoadAssetAtPath<Mesh>(TestAssetPaths.Mesh24verts);
            Assert.That(limitation.Check(obj), Is.False);
        }
    }
}

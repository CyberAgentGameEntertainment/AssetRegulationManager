// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl;
using NUnit.Framework;
using UnityEditor;

namespace AssetRegulationManager.Tests.Editor.AssetLimitationImpl
{
    internal sealed class MaxSceneGameObjectCountLimitationTest
    {
        [Test]
        public void Check_GameObjectCountIsEqualsToLimitation_ReturnTrue()
        {
            var limitation = new MaxSceneGameObjectCountLimitation();
            limitation.MaxCount = 3;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene3Obj);
            Assert.That(limitation.Check(obj), Is.True);
        }

        [Test]
        public void Check_FbxVertexCountIsGreaterThanLimitation_ReturnFalse()
        {
            var limitation = new MaxSceneGameObjectCountLimitation();
            limitation.MaxCount = 2;
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(TestAssetPaths.Scene3Obj);
            Assert.That(limitation.Check(obj), Is.False);
        }
    }
}

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetLimitationImpl
{
    internal sealed class MaxSceneTexelCountLimitationTest
    {
        [Test]
        public static void Check_CountIsEqualsToLimitation_ReturnTrue()
        {
            var limitation = new MaxSceneTexelCountLimitation();
            limitation.ExcludeInactive = false;
            limitation.AllowDuplicateCount = false;
            limitation.MaxCount = 64 * 64 + 128 * 128;
            var asset = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.SceneTexel64x2And128);

            Assert.That(limitation.Check(asset), Is.True);
        }

        [Test]
        public static void Check_CountIsGreaterThanLimitation_ReturnFalse()
        {
            var limitation = new MaxSceneTexelCountLimitation();
            limitation.ExcludeInactive = false;
            limitation.AllowDuplicateCount = false;
            limitation.MaxCount = 64 * 64 + 128 * 128 - 1;
            var asset = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.SceneTexel64x2And128);

            Assert.That(limitation.Check(asset), Is.False);
        }

        [Test]
        public static void Check_AllowDuplicateCountAndCountIsEqualsToLimitation_ReturnTrue()
        {
            var limitation = new MaxSceneTexelCountLimitation();
            limitation.ExcludeInactive = false;
            limitation.AllowDuplicateCount = true;
            limitation.MaxCount = 64 * 64 * 2 + 128 * 128;
            var asset = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.SceneTexel64x2And128);

            Assert.That(limitation.Check(asset), Is.True);
        }

        [Test]
        public static void Check_AllowDuplicateCountAndCountIsGreaterThanLimitation_ReturnFalse()
        {
            var limitation = new MaxSceneTexelCountLimitation();
            limitation.ExcludeInactive = false;
            limitation.AllowDuplicateCount = true;
            limitation.MaxCount = 64 * 64 * 2 + 128 * 128 - 1;
            var asset = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.SceneTexel64x2And128);

            Assert.That(limitation.Check(asset), Is.False);
        }
    }
}

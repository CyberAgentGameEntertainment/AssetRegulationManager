// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetLimitationImpl
{
    internal sealed class MaxTextureSizeLimitationTest
    {
        [Test]
        public void Check_TextureSizeIsEqualsToLimitation_ReturnTrue()
        {
            var limitation = new MaxTextureSizeLimitation();
            limitation.MaxSize = Vector2.one * 64;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64);
            Assert.That(limitation.Check(texture), Is.True);
        }

        [Test]
        public void Check_TextureSizeIsLessThanLimitation_ReturnTrue()
        {
            var limitation = new MaxTextureSizeLimitation();
            limitation.MaxSize = Vector2.one * 128;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64);
            Assert.That(limitation.Check(texture), Is.True);
        }

        [Test]
        public void Check_TextureSizeIsLargerThanLimitation_ReturnFalse()
        {
            var limitation = new MaxTextureSizeLimitation();
            limitation.MaxSize = Vector2.one * 32;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64);
            Assert.That(limitation.Check(texture), Is.False);
        }

        [Test]
        public void Check_TextureSizeIsLessThanLimitationBySettings_ReturnTrue()
        {
            var limitation = new MaxTextureSizeLimitation();
            limitation.MaxSize = Vector2.one * 100;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture128MaxSize64);
            Assert.That(limitation.Check(texture), Is.True);
        }
    }
}

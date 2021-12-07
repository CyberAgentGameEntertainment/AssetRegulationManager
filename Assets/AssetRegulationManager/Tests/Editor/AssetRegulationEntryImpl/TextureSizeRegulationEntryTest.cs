// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetRegulationEntryImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetRegulationEntryImpl
{
    public sealed class TextureSizeRegulationEntryTest
    {
        [Test]
        public void RunTest_SizeIsLessThanRegulation_ReturnTrue()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetKeys.Texture64);
            var entry = new TextureSizeRegulationEntry
            {
                MaxSize = Vector2.one * 100
            };
            var result = entry.RunTest(asset);
            Assert.That(result, Is.True);
        }

        [Test]
        public void RunTest_SizeIsEqualToRegulation_ReturnTrue()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetKeys.Texture64);
            var entry = new TextureSizeRegulationEntry
            {
                MaxSize = Vector2.one * 64
            };
            var result = entry.RunTest(asset);
            Assert.That(result, Is.True);
        }

        [Test]
        public void RunTest_SizeIsLargerThanRegulation_ReturnFalse()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetKeys.Texture128);
            var entry = new TextureSizeRegulationEntry
            {
                MaxSize = Vector2.one * 100
            };
            var result = entry.RunTest(asset);
            Assert.That(result, Is.False);
        }

        [Test]
        public void RunTest_SizeIsLessThanRegulationBySettings_ReturnTrue()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetKeys.Texture128MaxSize64);
            var entry = new TextureSizeRegulationEntry
            {
                MaxSize = Vector2.one * 100
            };
            var result = entry.RunTest(asset);
            Assert.That(result, Is.True);
        }
    }
}
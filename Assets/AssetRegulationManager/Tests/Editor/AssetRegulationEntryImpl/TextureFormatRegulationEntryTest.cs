// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetRegulationEntryImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetRegulationEntryImpl
{
    public sealed class TextureFormatRegulationEntryTest
    {
        [Test]
        public void RunTest_FormatMatches_ReturnTrue()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetKeys.Texture64iOSAstc6AndAstc4);
            var entry = new TextureFormatRegulationEntry
            {
                Target = BuildTargetGroup.iOS
            };
            entry.Formats.Add(TextureImporterFormat.ASTC_6x6);
            var result = entry.RunTest(asset);
            Assert.That(result, Is.True);
        }

        [Test]
        public void RunTest_FormatDontMatches_ReturnFalse()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetKeys.Texture64iOSAstc6AndAstc4);
            var entry = new TextureFormatRegulationEntry
            {
                Target = BuildTargetGroup.iOS
            };
            entry.Formats.Add(TextureImporterFormat.RGBA32);
            var result = entry.RunTest(asset);
            Assert.That(result, Is.False);
        }

        [Test]
        public void RunTest_CheckOtherPlatform_ReturnTrue()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetKeys.Texture64iOSAstc6AndAstc4);
            var entry = new TextureFormatRegulationEntry
            {
                Target = BuildTargetGroup.Android
            };
            entry.Formats.Add(TextureImporterFormat.ASTC_4x4);
            var result = entry.RunTest(asset);
            Assert.That(result, Is.True);
        }
    }
}

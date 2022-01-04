// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetLimitationImpl
{
    internal sealed class TextureFormatLimitationTest
    {
        [Test]
        public void Check_FormatMatches_ReturnTrue()
        {
            var limitation = new TextureFormatLimitation();
            limitation.Target.Value = BuildTargetGroup.iOS;
            limitation.Format.Value = TextureImporterFormat.ASTC_6x6;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(limitation.Check(texture), Is.True);
        }

        [Test]
        public void Check_FormatDontMatches_ReturnFalse()
        {
            var limitation = new TextureFormatLimitation();
            limitation.Target.Value = BuildTargetGroup.iOS;
            limitation.Format.Value = TextureImporterFormat.RGBA32;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(limitation.Check(texture), Is.False);
        }

        [Test]
        public void Check_CheckOtherPlatform_ReturnTrue()
        {
            var limitation = new TextureFormatLimitation();
            limitation.Target.Value = BuildTargetGroup.Android;
            limitation.Format.Value = TextureImporterFormat.ASTC_4x4;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(limitation.Check(texture), Is.True);
        }

        [Test]
        public void Check_TargetContainsNull_ReturnTrue()
        {
            var limitation = new TextureFormatLimitation();
            limitation.Target.IsListMode = true;
            limitation.Target.AddValue((BuildTargetGroup)(-1));
            limitation.Target.AddValue(BuildTargetGroup.iOS);
            limitation.Format.Value = TextureImporterFormat.ASTC_6x6;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(limitation.Check(texture), Is.True);
        }
    }
}

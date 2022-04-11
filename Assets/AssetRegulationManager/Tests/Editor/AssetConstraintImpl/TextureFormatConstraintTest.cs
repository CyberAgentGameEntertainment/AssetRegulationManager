// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class TextureFormatConstraintTest
    {
        [Test]
        public void Check_FormatMatches_ReturnTrue()
        {
            var constraint = new TextureFormatConstraint();
            constraint.Target.Value = BuildTargetGroup.iOS;
            constraint.Format.Value = TextureImporterFormat.ASTC_6x6;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(texture), Is.True);
        }

        [Test]
        public void Check_FormatDontMatches_ReturnFalse()
        {
            var constraint = new TextureFormatConstraint();
            constraint.Target.Value = BuildTargetGroup.iOS;
            constraint.Format.Value = TextureImporterFormat.RGBA32;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(texture), Is.False);
        }

        [Test]
        public void Check_CheckOtherPlatform_ReturnTrue()
        {
            var constraint = new TextureFormatConstraint();
            constraint.Target.Value = BuildTargetGroup.Android;
            constraint.Format.Value = TextureImporterFormat.ASTC_4x4;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(texture), Is.True);
        }

        [Test]
        public void Check_TargetContainsNull_ReturnTrue()
        {
            var constraint = new TextureFormatConstraint();
            constraint.Target.IsListMode = true;
            constraint.Target.AddValue((BuildTargetGroup)(-1));
            constraint.Target.AddValue(BuildTargetGroup.iOS);
            constraint.Format.Value = TextureImporterFormat.ASTC_6x6;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(texture), Is.True);
        }
    }
}

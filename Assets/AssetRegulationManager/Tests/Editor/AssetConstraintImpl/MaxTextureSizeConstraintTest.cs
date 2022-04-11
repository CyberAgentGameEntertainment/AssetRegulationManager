// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class MaxTextureSizeConstraintTest
    {
        [Test]
        public void Check_TextureSizeIsEqualsToConstraint_ReturnTrue()
        {
            var constraint = new MaxTextureSizeConstraint();
            constraint.CountMode = TextureSizeCountMode.WidthAndHeight;
            constraint.MaxSize = Vector2.one * 64;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64);
            Assert.That(constraint.Check(texture), Is.True);
        }

        [Test]
        public void Check_TextureSizeIsLessThanConstraint_ReturnTrue()
        {
            var constraint = new MaxTextureSizeConstraint();
            constraint.CountMode = TextureSizeCountMode.WidthAndHeight;
            constraint.MaxSize = Vector2.one * 128;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64);
            Assert.That(constraint.Check(texture), Is.True);
        }

        [Test]
        public void Check_TextureSizeIsLargerThanConstraint_ReturnFalse()
        {
            var constraint = new MaxTextureSizeConstraint();
            constraint.CountMode = TextureSizeCountMode.WidthAndHeight;
            constraint.MaxSize = Vector2.one * 32;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64);
            Assert.That(constraint.Check(texture), Is.False);
        }

        [Test]
        public void Check_TextureSizeIsLessThanConstraintBySettings_ReturnTrue()
        {
            var constraint = new MaxTextureSizeConstraint();
            constraint.CountMode = TextureSizeCountMode.WidthAndHeight;
            constraint.MaxSize = Vector2.one * 100;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture128MaxSize64);
            Assert.That(constraint.Check(texture), Is.True);
        }

        [Test]
        public void Check_TexelCountIsEqualsToConstraint_ReturnTrue()
        {
            var constraint = new MaxTextureSizeConstraint();
            constraint.CountMode = TextureSizeCountMode.TexelCount;
            constraint.MaxTexelCount = 64 * 64;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64);
            Assert.That(constraint.Check(texture), Is.True);
        }

        [Test]
        public void Check_TexelCountIsLessThanConstraint_ReturnTrue()
        {
            var constraint = new MaxTextureSizeConstraint();
            constraint.CountMode = TextureSizeCountMode.TexelCount;
            constraint.MaxTexelCount = 128 * 128;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64);
            Assert.That(constraint.Check(texture), Is.True);
        }

        [Test]
        public void Check_TexelCountIsLargerThanConstraint_ReturnFalse()
        {
            var constraint = new MaxTextureSizeConstraint();
            constraint.CountMode = TextureSizeCountMode.TexelCount;
            constraint.MaxTexelCount = 32 * 32;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture64);
            Assert.That(constraint.Check(texture), Is.False);
        }

        [Test]
        public void Check_TexelCountIsLessThanConstraintBySettings_ReturnTrue()
        {
            var constraint = new MaxTextureSizeConstraint();
            constraint.CountMode = TextureSizeCountMode.TexelCount;
            constraint.MaxTexelCount = 100 * 100;
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(TestAssetPaths.Texture128MaxSize64);
            Assert.That(constraint.Check(texture), Is.True);
        }
    }
}

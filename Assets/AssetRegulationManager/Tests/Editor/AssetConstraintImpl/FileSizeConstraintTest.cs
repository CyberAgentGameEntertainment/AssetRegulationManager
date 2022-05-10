using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class FileSizeConstraintTest
    {
        [Test]
        public void Check_FileSizeIsNotOver_ReturnTrue()
        {
            var constraint = new FileSizeConstraint();
            constraint.Size = 500;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_FileSizeIsOver_ReturnFalse()
        {
            var constraint = new FileSizeConstraint();
            constraint.Size = 400;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64);
            Assert.That(constraint.Check(obj), Is.False);
        }
        [Test]
        public void Check_UnitIsKB_FileSizeIsNotOver_ReturnTrue()
        {
            var constraint = new FileSizeConstraint();
            constraint.Size = 30;
            constraint.Unit = FileSizeConstraint.SizeUnit.KB;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture2048);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_UnitIsKB_FileSizeIsOver_ReturnFalse()
        {
            var constraint = new FileSizeConstraint();
            constraint.Size = 20;
            constraint.Unit = FileSizeConstraint.SizeUnit.KB;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture2048);
            Assert.That(constraint.Check(obj), Is.False);
        }
    }
}

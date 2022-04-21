using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class FolderConstraintTest
    {
        [Test]
        public void Check_Contains_ReturnTrue()
        {
            var constraint = new FolderConstraint();
            constraint.Folder.Value = AssetDatabase.LoadAssetAtPath<DefaultAsset>(TestAssetPaths.BaseFolderPath);
            constraint.TopFolderOnly = false;
            constraint.CheckMode = FolderConstraintCheckMode.Contains;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_NotContains_ReturnFalse()
        {
            var constraint = new FolderConstraint();
            constraint.Folder.Value = AssetDatabase.LoadAssetAtPath<DefaultAsset>(TestAssetPaths.DummyFolderPath);
            constraint.TopFolderOnly = false;
            constraint.CheckMode = FolderConstraintCheckMode.Contains;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.False);
        }
        
        [Test]
        public void Check_TopDirectoryOnly_Contains_ReturnTrue()
        {
            var constraint = new FolderConstraint();
            constraint.Folder.Value = AssetDatabase.LoadAssetAtPath<DefaultAsset>(TestAssetPaths.BaseFolderPath);
            constraint.TopFolderOnly = true;
            constraint.CheckMode = FolderConstraintCheckMode.Contains;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_TopDirectoryOnly_NotContains_ReturnFalse()
        {
            var constraint = new FolderConstraint();
            constraint.Folder.Value = AssetDatabase.LoadAssetAtPath<DefaultAsset>(TestAssetPaths.BaseFolderPath);
            constraint.TopFolderOnly = true;
            constraint.CheckMode = FolderConstraintCheckMode.Contains;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.DummyPrefabPath);
            Assert.That(constraint.Check(obj), Is.False);
        }
        
        [Test]
        public void Check_CheckNotContains_NotContains_ReturnTrue()
        {
            var constraint = new FolderConstraint();
            constraint.Folder.Value = AssetDatabase.LoadAssetAtPath<DefaultAsset>(TestAssetPaths.DummyFolderPath);
            constraint.TopFolderOnly = false;
            constraint.CheckMode = FolderConstraintCheckMode.NotContains;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_CheckNotContains_Contains_ReturnFalse()
        {
            var constraint = new FolderConstraint();
            constraint.Folder.Value = AssetDatabase.LoadAssetAtPath<DefaultAsset>(TestAssetPaths.BaseFolderPath);
            constraint.TopFolderOnly = false;
            constraint.CheckMode = FolderConstraintCheckMode.NotContains;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.False);
        }
    }
}

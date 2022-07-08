using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class FolderConstraintTest
    {
        [TestCase(FolderConstraintCheckMode.Contains, TestAssetPaths.BaseFolderPath, ExpectedResult = true)]
        [TestCase(FolderConstraintCheckMode.Contains, TestAssetPaths.DummyFolderPath, ExpectedResult = false)]
        [TestCase(FolderConstraintCheckMode.NotContains, TestAssetPaths.DummyFolderPath, ExpectedResult = true)]
        [TestCase(FolderConstraintCheckMode.NotContains, TestAssetPaths.BaseFolderPath, ExpectedResult = false)]
        public bool Check_CheckMode(FolderConstraintCheckMode checkMode, string folderPath)
        {
            var constraint = new FolderConstraint();
            constraint.Folder.Value = AssetDatabase.LoadAssetAtPath<DefaultAsset>(folderPath);
            constraint.TopFolderOnly = false;
            constraint.CheckMode = checkMode;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            return constraint.Check(obj);
        }

        [TestCase(true, TestAssetPaths.Texture64iOSAstc6AndAstc4, ExpectedResult = true)]
        [TestCase(true, TestAssetPaths.DummyPrefabPath, ExpectedResult = false)]
        [TestCase(false, TestAssetPaths.DummyPrefabPath, ExpectedResult = true)]
        public bool Check_TopFolderOnly(bool topFolderOnly, string targetAssetPath)
        {
            var constraint = new FolderConstraint();
            constraint.Folder.Value = AssetDatabase.LoadAssetAtPath<DefaultAsset>(TestAssetPaths.BaseFolderPath);
            constraint.TopFolderOnly = topFolderOnly;
            constraint.CheckMode = FolderConstraintCheckMode.Contains;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(targetAssetPath);
            return constraint.Check(obj);
        }

        [TestCase(TestAssetPaths.BaseFolderPath, ExpectedResult = false)]
        [TestCase(TestAssetPaths.DummyFolderPath, ExpectedResult = true)]
        public bool Check_TargetIsFolder(string targetAssetPath)
        {
            var constraint = new FolderConstraint();
            constraint.Folder.Value = AssetDatabase.LoadAssetAtPath<DefaultAsset>(TestAssetPaths.BaseFolderPath);
            constraint.TopFolderOnly = false;
            constraint.CheckMode = FolderConstraintCheckMode.Contains;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(targetAssetPath);
            return constraint.Check(obj);
        }
    }
}

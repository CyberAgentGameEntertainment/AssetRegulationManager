using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class FolderConstraintTest
    {
        [TestCase(FolderConstraintCheckMode.Contains, "", ExpectedResult = true)]
        [TestCase(FolderConstraintCheckMode.Contains, TestAssetRelativePaths.DummyFolder, ExpectedResult = false)]
        [TestCase(FolderConstraintCheckMode.NotContains, TestAssetRelativePaths.DummyFolder, ExpectedResult = true)]
        [TestCase(FolderConstraintCheckMode.NotContains, "", ExpectedResult = false)]
        public bool Check_CheckMode(FolderConstraintCheckMode checkMode, string folderRelativePath)
        {
            var folderPath = TestAssetPaths.CreateAbsoluteAssetPath(folderRelativePath);
            var constraint = new FolderConstraint();
            constraint.Folder.Value = AssetDatabase.LoadAssetAtPath<DefaultAsset>(folderPath);
            constraint.TopFolderOnly = false;
            constraint.CheckMode = checkMode;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            return constraint.Check(obj);
        }

        [TestCase(true, TestAssetRelativePaths.Texture64iOSAstc6AndAstc4, ExpectedResult = true)]
        [TestCase(true, TestAssetRelativePaths.DummyPrefab, ExpectedResult = false)]
        [TestCase(false, TestAssetRelativePaths.DummyPrefab, ExpectedResult = true)]
        public bool Check_TopFolderOnly(bool topFolderOnly, string targetAssetRelativePath)
        {
            var targetAssetPath = TestAssetPaths.CreateAbsoluteAssetPath(targetAssetRelativePath);
            var constraint = new FolderConstraint();
            constraint.Folder.Value = AssetDatabase.LoadAssetAtPath<DefaultAsset>(TestAssetPaths.Folder);
            constraint.TopFolderOnly = topFolderOnly;
            constraint.CheckMode = FolderConstraintCheckMode.Contains;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(targetAssetPath);
            return constraint.Check(obj);
        }

        [TestCase("", ExpectedResult = false)]
        [TestCase(TestAssetRelativePaths.DummyFolder, ExpectedResult = true)]
        public bool Check_TargetIsFolder(string targetAssetRelativePath)
        {
            var targetAssetPath = TestAssetPaths.CreateAbsoluteAssetPath(targetAssetRelativePath);
            var constraint = new FolderConstraint();
            constraint.Folder.Value = AssetDatabase.LoadAssetAtPath<DefaultAsset>(TestAssetPaths.Folder);
            constraint.TopFolderOnly = false;
            constraint.CheckMode = FolderConstraintCheckMode.Contains;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(targetAssetPath);
            return constraint.Check(obj);
        }
    }
}

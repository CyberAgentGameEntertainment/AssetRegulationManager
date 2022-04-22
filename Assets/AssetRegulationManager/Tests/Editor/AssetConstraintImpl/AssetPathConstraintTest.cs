using System.IO;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Core.Shared;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class AssetPathConstraintTest
    {
        [Test]
        public void Check_AssetPathMatches_ReturnTrue()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.Value = TestAssetPaths.Texture64iOSAstc6AndAstc4;
            constraint.PathType = AssetPathType.AssetPath;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_AssetPathNotMatches_ReturnFalse()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.Value = TestAssetPaths.Texture64;
            constraint.PathType = AssetPathType.AssetPath;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.False);
        }

        [Test]
        public void Check_AssetNameMatches_ReturnTrue()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.Value =
                AssetPathUtility.NormalizeAssetPath(Path.GetFileName(TestAssetPaths.Texture64iOSAstc6AndAstc4));
            constraint.PathType = AssetPathType.AssetName;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_AssetNameNotMatches_ReturnFalse()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.Value =
                AssetPathUtility.NormalizeAssetPath(Path.GetFileName(TestAssetPaths.Texture64));
            constraint.PathType = AssetPathType.AssetName;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.False);
        }

        [Test]
        public void Check_AssetNameWithoutExtensionsMatches_ReturnTrue()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.Value =
                AssetPathUtility.NormalizeAssetPath(
                    Path.GetFileNameWithoutExtension(TestAssetPaths.Texture64iOSAstc6AndAstc4));
            constraint.PathType = AssetPathType.AssetNameWithoutExtensions;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_AssetNameWithoutExtensionsNotMatches_ReturnFalse()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.Value =
                AssetPathUtility.NormalizeAssetPath(
                    Path.GetFileNameWithoutExtension(TestAssetPaths.Texture128MaxSize64));
            constraint.PathType = AssetPathType.AssetNameWithoutExtensions;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.False);
        }

        [Test]
        public void Check_FolderPathMatches_ReturnTrue()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.Value = TestAssetPaths.BaseFolderPath;
            constraint.PathType = AssetPathType.FolderPath;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_FolderPathNotMatches_ReturnFalse()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.Value = TestAssetPaths.BaseFolderPath + "/Dummy";
            constraint.PathType = AssetPathType.FolderPath;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.False);
        }

        [Test]
        public void Check_FolderNameMatches_ReturnTrue()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.Value = "TestAssets";
            constraint.PathType = AssetPathType.FolderName;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_FolderNameNotMatches_ReturnFalse()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.Value = "Dummy";
            constraint.PathType = AssetPathType.FolderName;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.False);
        }

        [Test]
        public void Check_CheckModeIsAnd_Matches_ReturnTrue()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.IsListMode = true;
            constraint.AssetPath.AddValue("^Assets/AssetRegulationManager");
            constraint.AssetPath.AddValue(TestAssetPaths.Texture64);
            constraint.PathType = AssetPathType.AssetPath;
            constraint.CheckMode = AssetPathConstraintCheckMode.And;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_CheckModeIsAnd_NotMatches_ReturnTrue()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.IsListMode = true;
            constraint.AssetPath.AddValue("^Assets/AssetRegulationManager");
            constraint.AssetPath.AddValue(TestAssetPaths.Texture128MaxSize64);
            constraint.PathType = AssetPathType.AssetPath;
            constraint.CheckMode = AssetPathConstraintCheckMode.And;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.False);
        }

        [Test]
        public void Check_CheckModeIsOr_Matches_ReturnTrue()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.IsListMode = true;
            constraint.AssetPath.AddValue("^Assets/AssetRegulationManager");
            constraint.AssetPath.AddValue(TestAssetPaths.Texture128MaxSize64);
            constraint.PathType = AssetPathType.AssetPath;
            constraint.CheckMode = AssetPathConstraintCheckMode.Or;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64);
            Assert.That(constraint.Check(obj), Is.True);
        }

        [Test]
        public void Check_CheckModeIsOr_NotMatches_ReturnTrue()
        {
            var constraint = new AssetPathConstraint();
            constraint.AssetPath.IsListMode = true;
            constraint.AssetPath.AddValue(TestAssetPaths.Texture64);
            constraint.AssetPath.AddValue(TestAssetPaths.Texture128MaxSize64);
            constraint.PathType = AssetPathType.AssetPath;
            constraint.CheckMode = AssetPathConstraintCheckMode.Or;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64iOSAstc6AndAstc4);
            Assert.That(constraint.Check(obj), Is.False);
        }
    }
}

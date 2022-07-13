using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class MaxFolderTexelCountConstraintTest
    {
        [TestCase(64 * 64 + 128 * 128 + 128 * 128, ExpectedResult = true)]
        [TestCase(64 * 64 + 128 * 128 + 128 * 128 - 1, ExpectedResult = false)]
        public static bool Check(int maxCount)
        {
            var constraint = new MaxFolderTexelCountConstraint();
            constraint.MaxCount = maxCount;
            var asset = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.TexelCountTestFolder);
            return constraint.Check(asset);
        }
        
        [TestCase(64 * 64 + 128 * 128, true, ExpectedResult = true)]
        [TestCase(64 * 64 + 128 * 128 - 1, true, ExpectedResult = false)]
        [TestCase(64 * 64 + 128 * 128, false, ExpectedResult = false)]
        public static bool Check_TopFolderOnly(int maxCount, bool topFolderOnly)
        {
            var constraint = new MaxFolderTexelCountConstraint();
            constraint.MaxCount = maxCount;
            constraint.TopFolderOnly = topFolderOnly;
            var asset = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.TexelCountTestFolder);
            return constraint.Check(asset);
        }
    }
}

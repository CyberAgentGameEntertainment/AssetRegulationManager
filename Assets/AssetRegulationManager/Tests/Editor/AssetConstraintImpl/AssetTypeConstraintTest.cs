using System;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Tests.Editor.AssetConstraintImpl
{
    internal sealed class AssetTypeConstraintTest
    {
        [TestCase(typeof(Texture2D), ExpectedResult = true)]
        [TestCase(typeof(Texture3D), ExpectedResult = false)]
        public bool Check(Type type)
        {
            var constraint = new AssetTypeConstraint();
            constraint.Type.Value = TypeReference.Create(type);
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64);
            return constraint.Check(obj);
        }

        [TestCase(typeof(Texture2D), true, ExpectedResult = true)]
        [TestCase(typeof(Texture), true, ExpectedResult = true)]
        [TestCase(typeof(Texture), false, ExpectedResult = false)]
        public bool Check_MatchWithDerivedTypes(Type type, bool matchWithDerivedTypes)
        {
            var constraint = new AssetTypeConstraint();
            constraint.Type.Value = TypeReference.Create(type);
            constraint.MatchWithDerivedTypes = matchWithDerivedTypes;
            var obj = AssetDatabase.LoadAssetAtPath<Object>(TestAssetPaths.Texture64);
            return constraint.Check(obj);
        }
    }
}

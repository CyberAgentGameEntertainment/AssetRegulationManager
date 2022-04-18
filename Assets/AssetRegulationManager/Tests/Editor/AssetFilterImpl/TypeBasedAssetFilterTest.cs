using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl;
using NUnit.Framework;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor.AssetFilterImpl
{
    internal sealed class TypeBasedAssetFilterTest
    {
        [Test]
        public void IsMatch_SetMatchedType_ReturnTrue()
        {
            var filter = new TypeBasedAssetFilter();
            filter.Type.Value = TypeReference.Create(typeof(Texture2D));
            filter.SetupForMatching();
            Assert.That(filter.IsMatch(null, typeof(Texture2D)), Is.True);
        }
        
        [Test]
        public void IsMatch_SetDerivedType_ReturnTrue()
        {
            var filter = new TypeBasedAssetFilter();
            filter.Type.Value = TypeReference.Create(typeof(Texture));
            filter.SetupForMatching();
            Assert.That(filter.IsMatch(null, typeof(Texture2D)), Is.True);
        }
        
        [Test]
        public void IsMatch_SetNotMatchedType_ReturnFalse()
        {
            var filter = new TypeBasedAssetFilter();
            filter.Type.Value = TypeReference.Create(typeof(Texture3D));
            filter.SetupForMatching();
            Assert.That(filter.IsMatch(null, typeof(Texture2D)), Is.False);
        }

        [Test]
        public void IsMatch_ContainsMatched_ReturnTrue()
        {
            var filter = new TypeBasedAssetFilter();
            filter.Type.IsListMode = true;
            filter.Type.AddValue(TypeReference.Create(typeof(Texture3D)));
            filter.Type.AddValue(TypeReference.Create(typeof(Texture2D)));
            filter.SetupForMatching();
            Assert.That(filter.IsMatch(null, typeof(Texture2D)), Is.True);
        }

        [Test]
        public void IsMatch_NotContainsMatched_ReturnTrue()
        {
            var filter = new TypeBasedAssetFilter();
            filter.Type.IsListMode = true;
            filter.Type.AddValue(TypeReference.Create(typeof(Texture3D)));
            filter.Type.AddValue(TypeReference.Create(typeof(Texture2D)));
            filter.SetupForMatching();
            Assert.That(filter.IsMatch(null, typeof(Texture2DArray)), Is.False);
        }
    }
}

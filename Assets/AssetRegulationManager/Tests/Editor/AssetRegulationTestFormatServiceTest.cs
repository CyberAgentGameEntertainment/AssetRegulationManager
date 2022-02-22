// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model;
using AssetRegulationManager.Editor.Core.Model.Adapters;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using NUnit.Framework;

namespace AssetRegulationManager.Tests.Editor
{
    public class AssetRegulationTestFormatServiceTest
    {
        [Test]
        public void Run_WithTestsOfStoreIsEmpty_ExcludeEmptyTests_FormatCollectionIsEmpty()
        {
            var store = new FakeAssetRegulationTestStore();
            var service = new AssetRegulationTestFormatService(store);
            var formatCollection = service.Run(true);
            
            Assert.That(formatCollection.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void Run_WithTestsOfStoreIsEmpty_IncludeEmptyTests_FormatCollectionIsEmpty()
        {
            var store = new FakeAssetRegulationTestStore();
            var service = new AssetRegulationTestFormatService(store);
            var formatCollection = service.Run(false);
            
            Assert.That(formatCollection.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void Run_ExcludeEmptyTests_FormatSuccessfully()
        {
            var service = CreateService();
            var formatCollection = service.Run(true);
            Assert.That(formatCollection.Count, Is.EqualTo(1));
            var test = formatCollection.ElementAt(0);
            Assert.That(test.Entries.Count, Is.EqualTo(1));
            Assert.That(test.Entries.Values.ElementAt(0).Description, Is.EqualTo("1"));
            Assert.That(test.AssetPath, Is.EqualTo("dummy"));
        }
        
        [Test]
        public void Run_IncludeEmptyTests_FormatSuccessfully()
        {
            var service = CreateService();
            var formatCollection = service.Run(false);
            
            Assert.That(formatCollection.Count, Is.EqualTo(2));
            var test = formatCollection.ElementAt(0);
            Assert.That(test.Entries.Count, Is.EqualTo(1));
            Assert.That(test.Entries.Values.ElementAt(0).Description, Is.EqualTo("1"));
            Assert.That(test.AssetPath, Is.EqualTo("dummy"));
            var emptyTest = formatCollection.ElementAt(1);
            Assert.That(emptyTest.Entries.Count, Is.EqualTo(0));
            Assert.That(emptyTest.AssetPath, Is.EqualTo("dummy2"));
        }
        
        private AssetRegulationTestFormatService CreateService()
        {
            var store = new FakeAssetRegulationTestStore();
            var test = new AssetRegulationTest("dummy", new FakeAssetDatabaseAdapter());
            var emptyTest = new AssetRegulationTest("dummy2", new FakeAssetDatabaseAdapter());
            test.AddEntry(new FakeAssetLimitation(true, "1"));
            store.AddTests(new[] { test, emptyTest });

            var service = new AssetRegulationTestFormatService(store);
            return service;
        }

        private class FakeAssetDatabaseAdapter : IAssetDatabaseAdapter
        {
            public IEnumerable<string> FindAssetPaths(string filter)
            {
                throw new NotImplementedException();
            }

            TAsset IAssetDatabaseAdapter.LoadAssetAtPath<TAsset>(string assetPath)
            {
                return default;
            }

            public string[] GetAllAssetPaths()
            {
                throw new NotImplementedException();
            }
        }
    }
}

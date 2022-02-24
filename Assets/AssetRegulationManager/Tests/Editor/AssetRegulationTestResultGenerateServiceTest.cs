using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model;
using AssetRegulationManager.Editor.Core.Model.Adapters;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using NUnit.Framework;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor
{
    internal sealed class AssetRegulationTestResultGenerateServiceTest
    {
        [Test]
        public void Run_TargetStatusIsSuccess_ExcludeEmptyTests_GenerateSuccessfully()
        {
            var store = CreateStore();
            var service = CreateService(store);
            store.SortTests(TestSortType.ExcludeEmptyTests);
            var targetStatus = new List<AssetRegulationTestStatus> { AssetRegulationTestStatus.Success };
            var resultCollection = service.Run(targetStatus);
            
            Assert.That(resultCollection.results.Count, Is.EqualTo(1));
            var entries = resultCollection.results[0].entries;
            Assert.That(entries.Count, Is.EqualTo(1));
            Assert.That(entries[0].description, Is.EqualTo("1"));
            Assert.That(entries[0].status, Is.EqualTo(AssetRegulationTestStatus.Success.ToString()));
        }
        
        [Test]
        public void Run_TargetStatusIsSuccess_IncludeEmptyTests_GenerateSuccessfully()
        {
            var store = CreateStore();
            var service = CreateService(store);
            store.SortTests(TestSortType.All);
            var targetStatus = new List<AssetRegulationTestStatus> { AssetRegulationTestStatus.Success };
            var resultCollection = service.Run(targetStatus);
            
            Assert.That(resultCollection.results.Count, Is.EqualTo(2));
            var entries = resultCollection.results[0].entries;
            Assert.That(entries.Count, Is.EqualTo(1));
            Assert.That(entries[0].description, Is.EqualTo("1"));
            Assert.That(entries[0].status, Is.EqualTo(AssetRegulationTestStatus.Success.ToString()));
            var emptyEntries = resultCollection.results[1].entries;
            Assert.That(emptyEntries.Count, Is.EqualTo(1));
            Assert.That(emptyEntries[0].status, Is.EqualTo(AssetRegulationTestStatus.Success.ToString()));
        }

        [Test]
        public void Run_TargetStatusIsFailed_ExcludeEmptyTests_GenerateSuccessfully()
        {
            var store = CreateStore();
            var service = CreateService(store);
            store.SortTests(TestSortType.ExcludeEmptyTests);
            var targetStatus = new List<AssetRegulationTestStatus> { AssetRegulationTestStatus.Failed };
            var resultCollection = service.Run(targetStatus);
            
            Assert.That(resultCollection.results.Count, Is.EqualTo(1));
            var entries = resultCollection.results[0].entries;
            Assert.That(entries.Count, Is.EqualTo(1));
            Assert.That(entries[0].description, Is.EqualTo("2"));
            Assert.That(entries[0].status, Is.EqualTo(AssetRegulationTestStatus.Failed.ToString()));
        }
        
        [Test]
        public void Run_TargetStatusIsFailed_IncludeEmptyTests_GenerateSuccessfully()
        {
            var store = CreateStore();
            var service = CreateService(store);
            store.SortTests(TestSortType.All);
            var targetStatus = new List<AssetRegulationTestStatus> { AssetRegulationTestStatus.Failed };
            var resultCollection = service.Run(targetStatus);
            
            Assert.That(resultCollection.results.Count, Is.EqualTo(2));
            var entries = resultCollection.results[0].entries;
            Assert.That(entries.Count, Is.EqualTo(1));
            Assert.That(entries[0].description, Is.EqualTo("2"));
            Assert.That(entries[0].status, Is.EqualTo(AssetRegulationTestStatus.Failed.ToString()));
            var emptyEntries = resultCollection.results[1].entries;
            Assert.That(emptyEntries.Count, Is.EqualTo(0));
        }

        [Test]
        public void Run_TargetStatusIsNone_ExcludeEmptyTests_GenerateSuccessfully()
        {
            var store = CreateStore();
            var service = CreateService(store);
            store.SortTests(TestSortType.ExcludeEmptyTests);
            var targetStatus = new List<AssetRegulationTestStatus> { AssetRegulationTestStatus.None };
            var resultCollection = service.Run(targetStatus);
            
            Assert.That(resultCollection.results.Count, Is.EqualTo(1));
            var entries = resultCollection.results[0].entries;
            Assert.That(entries.Count, Is.EqualTo(1));
            Assert.That(entries[0].description, Is.EqualTo("3"));
            Assert.That(entries[0].status, Is.EqualTo(AssetRegulationTestStatus.None.ToString()));
        }
        
        [Test]
        public void Run_TargetStatusIsNone_IncludeEmptyTests_GenerateSuccessfully()
        {
            var store = CreateStore();
            var service = CreateService(store);
            store.SortTests(TestSortType.All);
            var targetStatus = new List<AssetRegulationTestStatus> { AssetRegulationTestStatus.None };
            var resultCollection = service.Run(targetStatus);
            
            Assert.That(resultCollection.results.Count, Is.EqualTo(2));
            var entries = resultCollection.results[0].entries;
            Assert.That(entries.Count, Is.EqualTo(1));
            Assert.That(entries[0].description, Is.EqualTo("3"));
            Assert.That(entries[0].status, Is.EqualTo(AssetRegulationTestStatus.None.ToString()));
            var emptyEntries = resultCollection.results[1].entries;
            Assert.That(emptyEntries.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void Run_TargetStatusIsAll_ExcludeEmptyTests_GenerateSuccessfully()
        {
            var store = CreateStore();
            var service = CreateService(store);
            store.SortTests(TestSortType.ExcludeEmptyTests);
            var targetStatus = new List<AssetRegulationTestStatus>
            {
                AssetRegulationTestStatus.Success,
                AssetRegulationTestStatus.Failed,
                AssetRegulationTestStatus.None
            };
            var resultCollection = service.Run(targetStatus);
            
            Assert.That(resultCollection.results.Count, Is.EqualTo(1));
            var entries = resultCollection.results[0].entries;
            Assert.That(entries.Count, Is.EqualTo(3));
            Assert.That(entries.Any(x => x.status == AssetRegulationTestStatus.None.ToString()), Is.True);
            Assert.That(entries.Any(x => x.status == AssetRegulationTestStatus.Success.ToString()), Is.True);
            Assert.That(entries.Any(x => x.status == AssetRegulationTestStatus.Failed.ToString()), Is.True);
        }

        [Test]
        public void Run_TargetStatusIsAll_IncludeEmptyTests_GenerateSuccessfully()
        {
            var store = CreateStore();
            var service = CreateService(store);
            store.SortTests(TestSortType.All);
            var targetStatus = new List<AssetRegulationTestStatus>
            {
                AssetRegulationTestStatus.Success,
                AssetRegulationTestStatus.Failed,
                AssetRegulationTestStatus.None
            };
            var resultCollection = service.Run(targetStatus);
            
            Assert.That(resultCollection.results.Count, Is.EqualTo(2));
            var entries = resultCollection.results[0].entries;
            Assert.That(entries.Count, Is.EqualTo(3));
            Assert.That(entries.Any(x => x.status == AssetRegulationTestStatus.None.ToString()), Is.True);
            Assert.That(entries.Any(x => x.status == AssetRegulationTestStatus.Success.ToString()), Is.True);
            Assert.That(entries.Any(x => x.status == AssetRegulationTestStatus.Failed.ToString()), Is.True);
            var emptyEntries = resultCollection.results[1].entries;
            Assert.That(emptyEntries.Count, Is.EqualTo(1));
            Assert.That(emptyEntries[0].status, Is.EqualTo(AssetRegulationTestStatus.Success.ToString()));
        }

        private IAssetRegulationTestStore CreateStore()
        {
            var store = new FakeAssetRegulationTestStore();
            
            var test = new AssetRegulationTest("dummy", new FakeAssetDatabaseAdapter());
            var emptyTest = new AssetRegulationTest("dummy2", new FakeAssetDatabaseAdapter());
            var successEntryId = test.AddEntry(new FakeAssetLimitation(true, "1"));
            var failedEntryId = test.AddEntry(new FakeAssetLimitation(false, "2"));
            test.AddEntry(new FakeAssetLimitation(true, "3"));
            store.AddTests(new[] { test, emptyTest });

            // Execute fake tests.
            test.Run(new[] { successEntryId, failedEntryId });
            emptyTest.Run(new string[]{});

            return store;
        }

        private AssetRegulationTestResultGenerateService CreateService(IAssetRegulationTestStore store)
        {
            var service = new AssetRegulationTestResultGenerateService(store);
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

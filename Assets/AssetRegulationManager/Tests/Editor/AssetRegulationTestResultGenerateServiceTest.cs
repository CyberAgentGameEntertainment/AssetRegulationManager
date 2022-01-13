using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model;
using AssetRegulationManager.Editor.Core.Model.Adapters;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using NUnit.Framework;

namespace AssetRegulationManager.Tests.Editor
{
    internal sealed class AssetRegulationTestResultGenerateServiceTest
    {
        [Test]
        public void Run_TargetStatusIsSuccess_GenerateSuccessfully()
        {
            var service = CreateService();
            var targetStatus = new List<AssetRegulationTestStatus> { AssetRegulationTestStatus.Success };
            var resultCollection = service.Run(targetStatus);

            var entries = resultCollection.results[0].entries;
            Assert.That(entries.Count, Is.EqualTo(1));
            Assert.That(entries[0].description, Is.EqualTo("1"));
            Assert.That(entries[0].status, Is.EqualTo(AssetRegulationTestStatus.Success));
        }

        [Test]
        public void Run_TargetStatusIsFailed_GenerateSuccessfully()
        {
            var service = CreateService();
            var targetStatus = new List<AssetRegulationTestStatus> { AssetRegulationTestStatus.Failed };
            var resultCollection = service.Run(targetStatus);

            var entries = resultCollection.results[0].entries;
            Assert.That(entries.Count, Is.EqualTo(1));
            Assert.That(entries[0].description, Is.EqualTo("2"));
            Assert.That(entries[0].status, Is.EqualTo(AssetRegulationTestStatus.Failed));
        }

        [Test]
        public void Run_TargetStatusIsNone_GenerateSuccessfully()
        {
            var service = CreateService();
            var targetStatus = new List<AssetRegulationTestStatus> { AssetRegulationTestStatus.None };
            var resultCollection = service.Run(targetStatus);

            var entries = resultCollection.results[0].entries;
            Assert.That(entries.Count, Is.EqualTo(1));
            Assert.That(entries[0].description, Is.EqualTo("3"));
            Assert.That(entries[0].status, Is.EqualTo(AssetRegulationTestStatus.None));
        }

        [Test]
        public void Run_TargetStatusIsAll_GenerateSuccessfully()
        {
            var service = CreateService();
            var targetStatus = new List<AssetRegulationTestStatus>
            {
                AssetRegulationTestStatus.Success,
                AssetRegulationTestStatus.Failed,
                AssetRegulationTestStatus.None
            };
            var resultCollection = service.Run(targetStatus);

            var entries = resultCollection.results[0].entries;
            Assert.That(entries.Count, Is.EqualTo(3));
            Assert.That(entries.Any(x => x.status == AssetRegulationTestStatus.None), Is.True);
            Assert.That(entries.Any(x => x.status == AssetRegulationTestStatus.Success), Is.True);
            Assert.That(entries.Any(x => x.status == AssetRegulationTestStatus.Failed), Is.True);
        }

        private AssetRegulationTestResultGenerateService CreateService()
        {
            var store = new FakeAssetRegulationTestStore();
            var test = new AssetRegulationTest("dummy", new FakeAssetDatabaseAdapter());
            var successEntryId = test.AddEntry(new FakeAssetLimitation(true, "1"));
            var failedEntryId = test.AddEntry(new FakeAssetLimitation(false, "2"));
            test.AddEntry(new FakeAssetLimitation(true, "3"));
            store.AddTests(new[] { test });

            // Execute fake tests.
            var runSequence = test.CreateRunSequence(new[] { successEntryId, failedEntryId });
            foreach (var _ in runSequence)
            {
            }

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

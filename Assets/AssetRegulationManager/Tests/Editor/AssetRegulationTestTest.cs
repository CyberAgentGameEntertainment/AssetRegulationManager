// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.Adapters;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using NUnit.Framework;

namespace AssetRegulationManager.Tests.Editor
{
    public class AssetRegulationTestTest
    {
        [Test]
        public void CreateRunSequence_SuccessAll_TestStatusIsSuccess()
        {
            var test = new AssetRegulationTest("Assets/Dummy", new AssetDatabaseAdapter());
            var entryId1 = test.AddLimitation(new FakeAssetLimitation(true));
            var entryId2 = test.AddLimitation(new FakeAssetLimitation(true));
            var sequence = test.CreateRunSequence(new[] { entryId1, entryId2 });
            foreach (var _ in sequence)
            {
            }

            Assert.That(test.Entries[entryId1].Status.Value, Is.EqualTo(AssetRegulationTestStatus.Success));
            Assert.That(test.Entries[entryId2].Status.Value, Is.EqualTo(AssetRegulationTestStatus.Success));
            Assert.That(test.LatestStatus.Value, Is.EqualTo(AssetRegulationTestStatus.Success));
        }

        [Test]
        public void CreateRunSequence_SuccessPartially_TestStatusIsFailed()
        {
            var test = new AssetRegulationTest("Assets/Dummy", new AssetDatabaseAdapter());
            var entryId1 = test.AddLimitation(new FakeAssetLimitation(true));
            var entryId2 = test.AddLimitation(new FakeAssetLimitation(false));
            var sequence = test.CreateRunSequence(new[] { entryId1, entryId2 });
            foreach (var _ in sequence)
            {
            }

            Assert.That(test.Entries[entryId1].Status.Value, Is.EqualTo(AssetRegulationTestStatus.Success));
            Assert.That(test.Entries[entryId2].Status.Value, Is.EqualTo(AssetRegulationTestStatus.Failed));
            Assert.That(test.LatestStatus.Value, Is.EqualTo(AssetRegulationTestStatus.Failed));
        }

        [Test]
        public void CreateRunSequence_FailAll_TestStatusIsFailed()
        {
            var test = new AssetRegulationTest("Assets/Dummy", new AssetDatabaseAdapter());
            var entryId1 = test.AddLimitation(new FakeAssetLimitation(false));
            var entryId2 = test.AddLimitation(new FakeAssetLimitation(false));
            var sequence = test.CreateRunSequence(new[] { entryId1, entryId2 });
            foreach (var _ in sequence)
            {
            }

            Assert.That(test.Entries[entryId1].Status.Value, Is.EqualTo(AssetRegulationTestStatus.Failed));
            Assert.That(test.Entries[entryId2].Status.Value, Is.EqualTo(AssetRegulationTestStatus.Failed));
            Assert.That(test.LatestStatus.Value, Is.EqualTo(AssetRegulationTestStatus.Failed));
        }
    }
}

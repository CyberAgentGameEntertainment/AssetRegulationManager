// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using NUnit.Framework;
using UnityEngine;

namespace AssetRegulationManager.Tests.Editor
{
    public sealed class AssetRegulationTestEntryTest
    {
        [Test]
        public void Run_Success_StatusIsSuccess()
        {
            var regulationEntry = new FakeAssetRegulationEntry(true);
            var testEntry = new AssetRegulationTestEntry(regulationEntry);
            Assert.That(testEntry.Status.Value, Is.EqualTo(AssetRegulationTestStatus.None));
            testEntry.Run(new Object());
            Assert.That(testEntry.Status.Value, Is.EqualTo(AssetRegulationTestStatus.Success));
        }

        [Test]
        public void Run_Failed_StatusIsFailed()
        {
            var regulationEntry = new FakeAssetRegulationEntry(false);
            var testEntry = new AssetRegulationTestEntry(regulationEntry);
            Assert.That(testEntry.Status.Value, Is.EqualTo(AssetRegulationTestStatus.None));
            testEntry.Run(new Object());
            Assert.That(testEntry.Status.Value, Is.EqualTo(AssetRegulationTestStatus.Failed));
        }
    }
}
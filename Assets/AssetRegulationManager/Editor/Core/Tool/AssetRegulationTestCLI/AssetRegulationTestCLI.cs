using System;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model;
using AssetRegulationManager.Editor.Core.Model.Adapters;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationTestCLI
{
    public static class AssetRegulationTestCLI
    {
        private const int ErrorLevelNone = 0;
        private const int ErrorLevelTestFailed = 2;
        private const int ErrorLevelFailed = 3;

        public static void ExecuteTests()
        {
            try
            {
                var repository = new AssetRegulationRepository();
                var store = new AssetRegulationManagerStore(repository);
                var assetDatabaseAdapter = new AssetDatabaseAdapter();
                var testGenerateService = new AssetRegulationTestGenerateService(store, store, assetDatabaseAdapter);
                var testExecuteService = new AssetRegulationTestExecuteService(store);
                var testResultExportService = new AssetRegulationTestResultExportService(store);

                var options = AssetRegulationTestCLIOptions.CreateFromCommandLineArgs();

                // Create tests.
                testGenerateService.Run(options.AssetPathFilters, true, options.RegulationDescriptionFilters);

                // Execute tests.
                foreach (var _ in testExecuteService.CreateRunAllSequence())
                {
                }

                // Export test results.
                if (options.AsJson)
                {
                    testResultExportService.RunAsJson(options.ResultFilePath, options.TargetStatusList);
                }
                else
                {
                    testResultExportService.Run(options.ResultFilePath, options.TargetStatusList);
                }

                // Exit and return code.
                if (store.Tests.Values.Any(x => x.LatestStatus.Value == AssetRegulationTestStatus.Failed))
                {
                    EditorApplication.Exit(ErrorLevelTestFailed);
                }
                else
                {
                    EditorApplication.Exit(ErrorLevelNone);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                EditorApplication.Exit(ErrorLevelFailed);
            }

            EditorApplication.Exit(ErrorLevelNone);
        }
    }
}

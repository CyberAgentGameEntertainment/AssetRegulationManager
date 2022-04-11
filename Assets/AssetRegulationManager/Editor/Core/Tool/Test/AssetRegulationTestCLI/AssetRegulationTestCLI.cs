using System;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model;
using AssetRegulationManager.Editor.Core.Model.Adapters;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.Test.AssetRegulationTestCLI
{
    public static class AssetRegulationTestCLI
    {
        private const int ErrorLevelNone = 0;
        private const int ErrorLevelTestFailed = 1;
        private const int ErrorLevelFailed = 2;

        public static void ExecuteTests()
        {
            try
            {
                var repository = new AssetRegulationRepository();
                var testStore = new AssetRegulationTestStore();
                var assetDatabaseAdapter = new AssetDatabaseAdapter();
                var testGenerateService =
                    new AssetRegulationTestGenerateService(repository, testStore, assetDatabaseAdapter);
                var testExecuteService = new AssetRegulationTestExecuteService(testStore);
                var testResultExportService = new AssetRegulationTestResultExportService(testStore);

                var options = AssetRegulationTestCLIOptions.CreateFromCommandLineArgs();
                
                // Create tests.
                testGenerateService.Run(options.AssetPathFilters, options.RegulationDescriptionFilters);
                
                // Filter tests.
                testStore.FilterTests(AssetRegulationTestStoreFilter.ExcludeEmptyTests);

                // Execute tests.
                testExecuteService.RunAll();

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
                if (testStore.FilteredTests.Any(x => x.LatestStatus.Value == AssetRegulationTestStatus.Failed))
                {
                    EditorApplication.Exit(ErrorLevelTestFailed);
                }
                else if (options.FailWhenWarning &&
                         testStore.FilteredTests.Any(x => x.LatestStatus.Value == AssetRegulationTestStatus.Warning))
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
        }
    }
}

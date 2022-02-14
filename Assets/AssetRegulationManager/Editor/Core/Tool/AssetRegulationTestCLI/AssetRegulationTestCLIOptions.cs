using System;
using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using AssetRegulationManager.Editor.Foundation;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationTestCLI
{
    public sealed class AssetRegulationTestCLIOptions
    {
        private const string ResultFilePathArgName = "-resultFilePath";
        private const string ResultFormatArgName = "-resultFormat";
        private const string ResultFilterArgName = "-resultFilter";
        private const string AssetFilterArgName = "-assetFilter";
        private const string RegulationFilterArgName = "-regulationFilter";
        private const string FailWhenWarningArgName = "-failWhenWarning";
        private const string ExcludeEmptyTestsArgName = "-excludeEmptyTest";
        private const string DefaultResultFilePathWithoutExtensions = "AssetRegulationManager/test_result";
        private readonly List<string> _assetPathFilters = new List<string>();
        private readonly List<string> _regulationDescriptionFilters = new List<string>();
        private readonly List<AssetRegulationTestStatus> _targetStatusList = new List<AssetRegulationTestStatus>();

        public string ResultFilePath { get; private set; }
        public bool AsJson { get; private set; }
        public IReadOnlyList<AssetRegulationTestStatus> TargetStatusList => _targetStatusList;
        public IReadOnlyList<string> AssetPathFilters => _assetPathFilters;
        public IReadOnlyList<string> RegulationDescriptionFilters => _regulationDescriptionFilters;
        public bool FailWhenWarning { get; private set; }
        public bool ExcludeEmptyTests { get; private set; } = true;

        public static AssetRegulationTestCLIOptions CreateFromCommandLineArgs()
        {
            var options = new AssetRegulationTestCLIOptions();

            // Result Format
            CommandLineUtility.TryGetStringValue(ResultFormatArgName, out var resultFormatText);
            options.AsJson = resultFormatText == "Json";

            // Result File Path
            if (!CommandLineUtility.TryGetStringValue(ResultFilePathArgName, out var resultFilePath))
            {
                var extension = options.AsJson ? ".json" : ".txt";
                resultFilePath = DefaultResultFilePathWithoutExtensions + extension;
            }

            options.ResultFilePath = resultFilePath;

            // Result Filter
            CommandLineUtility.TryGetStringValue(ResultFilterArgName, out var resultFilterText);
            if (!string.IsNullOrEmpty(resultFilterText))
            {
                foreach (var resultFilter in resultFilterText.Split(';'))
                {
                    if (resultFilter == "Success")
                    {
                        options._targetStatusList.Add(AssetRegulationTestStatus.Success);
                    }
                    else if (resultFilter == "Failed")
                    {
                        options._targetStatusList.Add(AssetRegulationTestStatus.Failed);
                    }
                    else if (resultFilter == "Warning")
                    {
                        options._targetStatusList.Add(AssetRegulationTestStatus.Warning);
                    }
                }
            }

            if (options._targetStatusList.Count == 0)
            {
                foreach (AssetRegulationTestStatus status in Enum.GetValues(typeof(AssetRegulationTestStatus)))
                {
                    if (status == AssetRegulationTestStatus.None)
                    {
                        continue;
                    }

                    options._targetStatusList.Add(status);
                }
            }

            // Asset Filter
            CommandLineUtility.TryGetStringValue(AssetFilterArgName, out var assetFilterText);
            if (!string.IsNullOrEmpty(assetFilterText))
            {
                foreach (var assetFilter in assetFilterText.Split(';'))
                {
                    options._assetPathFilters.Add(assetFilter);
                }
            }

            // Regulation Filter
            CommandLineUtility.TryGetStringValue(RegulationFilterArgName, out var regulationFilterText);
            if (!string.IsNullOrEmpty(regulationFilterText))
            {
                foreach (var regulationFilter in regulationFilterText.Split(';'))
                {
                    options._regulationDescriptionFilters.Add(regulationFilter);
                }
            }
            
            // Exclude Empty Tests
            CommandLineUtility.TryGetStringValue(ExcludeEmptyTestsArgName, out var excludeEmptyTestsText);
            if (!string.IsNullOrEmpty(excludeEmptyTestsText))
            {
                if (bool.TryParse(excludeEmptyTestsText, out var excludeEmptyTests))
                {
                    options.ExcludeEmptyTests = excludeEmptyTests;
                }
            }

            // Fail When Warning
            options.FailWhenWarning = CommandLineUtility.Contains(FailWhenWarningArgName);

            return options;
        }
    }
}

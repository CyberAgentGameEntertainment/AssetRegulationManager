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
        private const string DefaultResultFilePathWithoutExtensions = "AssetRegulationManager/test_result";
        private readonly List<string> _assetPathFilters = new List<string>();
        private readonly List<string> _regulationDescriptionFilters = new List<string>();
        private readonly List<AssetRegulationTestStatus> _targetStatusList = new List<AssetRegulationTestStatus>();

        public string ResultFilePath { get; private set; }
        public bool AsJson { get; private set; }
        public IReadOnlyList<AssetRegulationTestStatus> TargetStatusList => _targetStatusList;
        public IReadOnlyList<string> AssetPathFilters => _assetPathFilters;
        public IReadOnlyList<string> RegulationDescriptionFilters => _regulationDescriptionFilters;

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
            if (resultFilterText == "Success")
            {
                options._targetStatusList.Add(AssetRegulationTestStatus.Success);
            }
            else if (resultFilterText == "Failed")
            {
                options._targetStatusList.Add(AssetRegulationTestStatus.Failed);
            }
            else
            {
                options._targetStatusList.Add(AssetRegulationTestStatus.Success);
                options._targetStatusList.Add(AssetRegulationTestStatus.Failed);
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

            return options;
        }
    }
}

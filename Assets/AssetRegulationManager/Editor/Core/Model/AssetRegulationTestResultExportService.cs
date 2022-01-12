using System;
using System.IO;
using System.Text;
using AssetRegulationManager.Editor.Core.Data;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model
{
    public sealed class AssetRegulationTestResultExportService
    {
        private readonly AssetRegulationTestResultGenerateService _generateService;

        public AssetRegulationTestResultExportService(AssetRegulationManagerStore store)
        {
            _generateService = new AssetRegulationTestResultGenerateService(store);
        }

        public void Run(string exportPath)
        {
            var resultCollection = _generateService.Run();

            var resultText = new StringBuilder();
            foreach (var text in resultCollection.GetAsTexts())
            {
                if (resultText.Length >= 1)
                {
                    resultText.Append(Environment.NewLine);
                }

                resultText.Append(text);
            }

            ExportText(resultText.ToString(), exportPath);
        }

        public void RunAsJson(string exportPath)
        {
            var resultCollection = _generateService.Run();
            var json = JsonUtility.ToJson(resultCollection);
            ExportText(json, exportPath);
        }

        private static void ExportText(string text, string exportPath)
        {
            File.WriteAllText(text, exportPath);
        }
    }
}

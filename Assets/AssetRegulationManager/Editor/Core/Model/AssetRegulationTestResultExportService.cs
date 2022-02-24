﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.AssetRegulationTests;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model
{
    public sealed class AssetRegulationTestResultExportService
    {
        private readonly AssetRegulationTestResultGenerateService _generateService;

        public AssetRegulationTestResultExportService(IAssetRegulationTestStore store)
        {
            _generateService = new AssetRegulationTestResultGenerateService(store);
        }

        public void Run(string filePath, IReadOnlyList<AssetRegulationTestStatus> targetStatus = null)
        {
            var resultCollection = _generateService.Run(targetStatus);

            var resultText = new StringBuilder();
            foreach (var text in resultCollection.GetAsTexts())
            {
                if (resultText.Length >= 1)
                {
                    resultText.Append(Environment.NewLine);
                }

                resultText.Append(text);
            }

            ExportText(resultText.ToString(), filePath);
        }

        public void RunAsJson(string filePath, IReadOnlyList<AssetRegulationTestStatus> targetStatusList = null)
        {
            var resultCollection = _generateService.Run(targetStatusList);
            var json = JsonUtility.ToJson(resultCollection);
            ExportText(json, filePath);
        }

        private static void ExportText(string text, string filePath)
        {
            var folderPath = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            File.WriteAllText(filePath, text);
        }
    }
}

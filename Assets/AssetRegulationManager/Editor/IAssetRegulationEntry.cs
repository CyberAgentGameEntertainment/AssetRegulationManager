// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEngine;

namespace AssetRegulationManager.Editor
{
    public interface IAssetRegulationEntry
    {
        string Label { get; }
        string Explanation { get; }
        bool RunTest(Object obj);
        void DrawGUI();
    }
}
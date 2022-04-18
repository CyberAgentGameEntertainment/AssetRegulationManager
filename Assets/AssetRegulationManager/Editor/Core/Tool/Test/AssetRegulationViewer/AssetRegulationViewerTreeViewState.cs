// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.Test.AssetRegulationViewer
{
    [Serializable]
    internal sealed class AssetRegulationViewerTreeViewState : TreeViewState
    {
        [SerializeField] private MultiColumnHeaderState.Column[] _columnStates;

        public MultiColumnHeaderState.Column[] ColumnStates => _columnStates;

        public AssetRegulationViewerTreeViewState()
        {
            _columnStates = GetColumnStates();
        }

        private MultiColumnHeaderState.Column[] GetColumnStates()
        {
            var testColumn = new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Asset / Test"),
                headerTextAlignment = TextAlignment.Center,
                canSort = false,
                width = 400,
                minWidth = 150,
                autoResize = false,
                allowToggleVisibility = false
            };
            var actualValueColumn = new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent("Message"),
                headerTextAlignment = TextAlignment.Center,
                canSort = false,
                width = 400,
                minWidth = 50,
                autoResize = true,
                allowToggleVisibility = true
            };
            return new[] { testColumn, actualValueColumn };
        }
    }
}

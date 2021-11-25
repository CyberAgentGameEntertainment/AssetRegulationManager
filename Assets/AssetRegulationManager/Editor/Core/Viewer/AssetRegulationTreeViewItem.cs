// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using UnityEditor.IMGUI.Controls;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    internal sealed class AssetRegulationTreeViewItem : TreeViewItem
    {
        internal AssetRegulationTreeViewItem(RegulationMetaDatum metaDatum, string explanation,
            TestResultType resultType)
        {
            MetaDatum = metaDatum;
            Explanation = explanation;
            ResultType = resultType;
        }

        internal RegulationMetaDatum MetaDatum { get; }
        internal string Explanation { get; }
        internal TestResultType ResultType { get; set; }
    }
}
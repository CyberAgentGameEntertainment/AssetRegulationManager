// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System.IO;
using AssetRegulationManager.Editor.Foundation.Observable.ObservableProperty;
using UnityEditor.IMGUI.Controls;

namespace AssetRegulationManager.Editor.Core.Viewer
{
    public class AssetRegulationTreeViewItem : TreeViewItem
    {
        internal AssetRegulationTreeViewItem(RegulationMetaDatum metaDatum, string explanation, TestResultType resultType)
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
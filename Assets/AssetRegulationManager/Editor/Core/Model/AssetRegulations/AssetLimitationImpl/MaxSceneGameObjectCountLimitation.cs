// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Foundation.SelectableSerializeReference;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl
{
    [Serializable]
    [SelectableSerializeReferenceLabel("Max GameObject Count (Scene)")]
    public sealed class MaxSceneGameObjectCountLimitation : MaxSceneComponentCountLimitation<Transform>
    {
        public override string GetDescription()
        {
            var desc = $"Max GameObject Count in Scene: {MaxCount}";
            return desc;
        }
    }
}

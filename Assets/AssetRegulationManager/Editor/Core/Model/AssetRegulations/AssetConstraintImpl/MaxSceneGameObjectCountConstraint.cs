// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("Game Object/Max Game Object Count in Scene", "Max Game Object Count in Scene")]
    public sealed class MaxSceneGameObjectCountConstraint : MaxSceneComponentCountConstraint<Transform>
    {
        public override string GetDescription()
        {
            var desc =
                $"Max GameObject Count in Scene: {MaxCount} ({(ExcludeInactive ? "Exclude" : "Include")} Inactive)";
            return desc;
        }
    }
}

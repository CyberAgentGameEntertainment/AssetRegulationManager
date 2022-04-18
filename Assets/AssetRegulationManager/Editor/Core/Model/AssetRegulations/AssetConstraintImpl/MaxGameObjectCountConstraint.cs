// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("Game Object/Max Game Object Count in GameObject", "Max Game Object Count in GameObject")]
    public sealed class MaxGameObjectCountConstraint : MaxComponentCountConstraint<Transform>
    {
        public override string GetDescription()
        {
            var desc = $"Max GameObject Count: {MaxCount} ({(ExcludeInactive ? "Exclude" : "Include")} Inactive)";
            return desc;
        }
    }
}

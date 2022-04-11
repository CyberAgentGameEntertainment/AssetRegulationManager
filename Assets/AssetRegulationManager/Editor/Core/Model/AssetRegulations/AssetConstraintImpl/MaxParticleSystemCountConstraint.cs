// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("Particle System/Max Particle System Count in GameObject",
        "Max Particle System Count in GameObject")]
    public sealed class MaxParticleSystemCountConstraint : MaxComponentCountConstraint<ParticleSystem>
    {
    }
}

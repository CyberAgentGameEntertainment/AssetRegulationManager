// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("Particle System/Max Particle System Count in Scene", "Max Particle System Count in Scene")]
    public sealed class MaxSceneParticleSystemCountConstraint : MaxSceneComponentCountConstraint<ParticleSystem>
    {
    }
}

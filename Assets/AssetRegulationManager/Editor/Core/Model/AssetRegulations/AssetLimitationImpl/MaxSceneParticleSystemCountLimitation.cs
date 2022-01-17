// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using AssetRegulationManager.Editor.Foundation.SelectableSerializeReference;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl
{
    [Serializable]
    [SelectableSerializeReferenceLabel("Max ParticleSystem Count (Scene)")]
    public sealed class MaxSceneParticleSystemCountLimitation : MaxSceneComponentCountLimitation<ParticleSystem>
    {
    }
}

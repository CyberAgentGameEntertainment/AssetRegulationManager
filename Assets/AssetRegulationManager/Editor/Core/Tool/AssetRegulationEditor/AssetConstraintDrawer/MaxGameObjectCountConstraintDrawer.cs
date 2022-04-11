﻿using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(MaxGameObjectCountConstraint))]
    internal sealed class MaxGameObjectCountConstraintDrawer : MaxComponentCountConstraintDrawer<Transform>
    {
    }
}

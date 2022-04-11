using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(MaxSceneGameObjectCountConstraint))]
    internal sealed class MaxSceneGameObjectCountConstraintDrawer : MaxSceneComponentCountConstraintDrawer<Transform>
    {
    }
}

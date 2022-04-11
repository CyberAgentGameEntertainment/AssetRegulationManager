using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    internal abstract class MaxSceneComponentCountConstraintDrawer<TComponent>
        : GUIDrawer<MaxSceneComponentCountConstraint<TComponent>> where TComponent : Component
    {
        protected override void GUILayout(MaxSceneComponentCountConstraint<TComponent> target)
        {
            target.MaxCount =
                EditorGUILayout.IntField(ObjectNames.NicifyVariableName(nameof(target.MaxCount)),
                    target.MaxCount);
            target.ExcludeInactive =
                EditorGUILayout.Toggle(ObjectNames.NicifyVariableName(nameof(target.ExcludeInactive)),
                    target.ExcludeInactive);
        }
    }
}

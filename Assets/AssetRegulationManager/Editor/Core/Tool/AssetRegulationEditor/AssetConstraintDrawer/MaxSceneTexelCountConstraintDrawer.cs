using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(MaxSceneTexelCountConstraint))]
    internal sealed class MaxSceneTexelCountConstraintDrawer : GUIDrawer<MaxSceneTexelCountConstraint>
    {
        protected override void GUILayout(MaxSceneTexelCountConstraint target)
        {
            target.MaxCount =
                EditorGUILayout.IntField(ObjectNames.NicifyVariableName(nameof(target.MaxCount)),
                    target.MaxCount);
            target.ExcludeInactive =
                EditorGUILayout.Toggle(ObjectNames.NicifyVariableName(nameof(target.ExcludeInactive)),
                    target.ExcludeInactive);
            target.AllowDuplicateCount =
                EditorGUILayout.Toggle(ObjectNames.NicifyVariableName(nameof(target.AllowDuplicateCount)),
                    target.AllowDuplicateCount);
        }
    }
}

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(MaxSceneVertexCountConstraint))]
    internal sealed class MaxSceneVertexCountConstraintDrawer : GUIDrawer<MaxSceneVertexCountConstraint>
    {
        protected override void GUILayout(MaxSceneVertexCountConstraint target)
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

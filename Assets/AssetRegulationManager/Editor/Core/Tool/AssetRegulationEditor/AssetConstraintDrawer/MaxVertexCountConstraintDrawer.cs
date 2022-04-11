using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(MaxVertexCountConstraint))]
    internal sealed class MaxVertexCountConstraintDrawer : GUIDrawer<MaxVertexCountConstraint>
    {
        protected override void GUILayout(MaxVertexCountConstraint target)
        {
            target.MaxCount =
                EditorGUILayout.IntField(ObjectNames.NicifyVariableName(nameof(target.MaxCount)),
                    target.MaxCount);
            target.ExcludeChildren =
                EditorGUILayout.Toggle(ObjectNames.NicifyVariableName(nameof(target.ExcludeChildren)),
                    target.ExcludeChildren);
            target.ExcludeInactive =
                EditorGUILayout.Toggle(ObjectNames.NicifyVariableName(nameof(target.ExcludeInactive)),
                    target.ExcludeInactive);
            target.AllowDuplicateCount =
                EditorGUILayout.Toggle(ObjectNames.NicifyVariableName(nameof(target.AllowDuplicateCount)),
                    target.AllowDuplicateCount);
        }
    }
}

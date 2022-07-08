using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(MaxFolderTexelCountConstraint))]
    internal sealed class MaxFolderTexelCountConstraintDrawer : GUIDrawer<MaxFolderTexelCountConstraint>
    {
        protected override void GUILayout(MaxFolderTexelCountConstraint target)
        {
            target.MaxCount =
                EditorGUILayout.IntField(ObjectNames.NicifyVariableName(nameof(target.MaxCount)),
                    target.MaxCount);
            target.TopFolderOnly =
                EditorGUILayout.Toggle(ObjectNames.NicifyVariableName(nameof(target.TopFolderOnly)),
                    target.TopFolderOnly);
        }
    }
}

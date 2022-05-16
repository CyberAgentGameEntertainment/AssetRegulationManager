using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(MaxAssetTexelCountConstraint))]
    internal sealed class MaxAssetTexelCountConstraintDrawer : GUIDrawer<MaxAssetTexelCountConstraint>
    {
        protected override void GUILayout(MaxAssetTexelCountConstraint target)
        {
            target.MaxCount = EditorGUILayout.IntField(ObjectNames.NicifyVariableName(nameof(target.MaxCount)),
                target.MaxCount);
        }
    }
}

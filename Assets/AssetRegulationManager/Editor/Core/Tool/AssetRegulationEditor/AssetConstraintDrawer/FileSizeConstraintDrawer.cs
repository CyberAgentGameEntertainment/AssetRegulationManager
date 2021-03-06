using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(FileSizeConstraint))]
    internal sealed class FileSizeConstraintDrawer : GUIDrawer<FileSizeConstraint>
    {
        protected override void GUILayout(FileSizeConstraint target)
        {
            target.MaxSize = EditorGUILayout.LongField("Max Size", target.MaxSize);
            target.Unit = (FileSizeConstraint.SizeUnit)EditorGUILayout.EnumPopup("Unit", target.Unit);
        }
    }
}

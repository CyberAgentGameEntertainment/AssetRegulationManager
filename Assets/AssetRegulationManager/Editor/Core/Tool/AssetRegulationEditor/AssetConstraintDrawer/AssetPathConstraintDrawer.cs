using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(AssetPathConstraint))]
    internal class AssetPathConstraintDrawer : GUIDrawer<AssetPathConstraint>
    {
        private ListablePropertyGUI<string> _assetPathGUI;

        public override void Setup(object target)
        {
            base.Setup(target);
            _assetPathGUI = new TextListablePropertyGUI("Asset Path (Regex)", Target.AssetPath);
        }

        protected override void GUILayout(AssetPathConstraint target)
        {
            target.PathType = (AssetPathType)EditorGUILayout.EnumPopup("Path Type", target.PathType);
            using (new EditorGUI.DisabledScope(!target.AssetPath.IsListMode))
            {
                target.CheckMode =
                    (AssetPathConstraintCheckMode)EditorGUILayout.EnumPopup("Check Mode", target.CheckMode);
            }

            _assetPathGUI.DoLayout();
        }
    }
}

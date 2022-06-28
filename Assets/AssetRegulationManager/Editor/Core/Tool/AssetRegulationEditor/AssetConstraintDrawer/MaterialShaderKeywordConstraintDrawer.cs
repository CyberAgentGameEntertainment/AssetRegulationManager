using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(MaterialShaderKeywordConstraint))]
    internal class MaterialShaderKeywordConstraintDrawer : GUIDrawer<MaterialShaderKeywordConstraint>
    {
        private ListablePropertyGUI<string> _assetPathGUI;

        public override void Setup(object target)
        {
            base.Setup(target);
            _assetPathGUI = new TextListablePropertyGUI("Shader Keyword", Target.Keywords);
        }

        protected override void GUILayout(MaterialShaderKeywordConstraint target)
        {
            using (new EditorGUI.DisabledScope(!target.Keywords.IsListMode))
            {
                target.Condition =
                    (MaterialShaderKeywordConstraint.CheckCondition)EditorGUILayout.EnumPopup("Check Condition",
                        target.Condition);
            }

            _assetPathGUI.DoLayout();
        }
    }
}

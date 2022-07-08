using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(AssetTypeConstraint))]
    internal class AssetTypeConstraintDrawer : GUIDrawer<AssetTypeConstraint>
    {
        private TypeReferenceListablePropertyGUI _listablePropertyGUI;

        public override void Setup(object target)
        {
            base.Setup(target);
            _listablePropertyGUI = new TypeReferenceListablePropertyGUI("Type", Target.Type);
        }

        protected override void GUILayout(AssetTypeConstraint target)
        {
            target.MatchWithDerivedTypes = EditorGUILayout.Toggle(
                ObjectNames.NicifyVariableName(nameof(Target.MatchWithDerivedTypes)), Target.MatchWithDerivedTypes);
            _listablePropertyGUI.DoLayout();
        }
    }
}

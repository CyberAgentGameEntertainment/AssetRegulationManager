using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetFilterDrawer
{
    [CustomGUIDrawer(typeof(TypeBasedAssetFilter))]
    internal sealed class TypeBasedAssetFilterDrawer : GUIDrawer<TypeBasedAssetFilter>
    {
        private TypeReferenceListablePropertyGUI _listablePropertyGUI;

        public override void Setup(object target)
        {
            base.Setup(target);
            _listablePropertyGUI = new TypeReferenceListablePropertyGUI("Type", Target.Type);
        }

        protected override void GUILayout(TypeBasedAssetFilter target)
        {
            target.MatchWithDerivedTypes = EditorGUILayout.Toggle(
                ObjectNames.NicifyVariableName(nameof(Target.MatchWithDerivedTypes)), Target.MatchWithDerivedTypes);
            _listablePropertyGUI.DoLayout();
        }
    }
}

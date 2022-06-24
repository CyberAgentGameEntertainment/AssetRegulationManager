using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetFilterDrawer
{
    [CustomGUIDrawer(typeof(DependentObjectBasedAssetFilter))]
    internal sealed class DependentObjectBasedAssetFilterDrawer : GUIDrawer<DependentObjectBasedAssetFilter>
    {
        private ObjectListablePropertyGUI _listablePropertyGUI;

        public override void Setup(object target)
        {
            base.Setup(target);
            _listablePropertyGUI = new ObjectListablePropertyGUI(ObjectNames.NicifyVariableName(nameof(Target.Object)),
                Target.Object, typeof(Object), false);
        }

        protected override void GUILayout(DependentObjectBasedAssetFilter target)
        {
            target.OnlyDirectDependencies =
                EditorGUILayout.Toggle("Only Direct Dependencies", target.OnlyDirectDependencies);
            _listablePropertyGUI.DoLayout();
        }
    }
}

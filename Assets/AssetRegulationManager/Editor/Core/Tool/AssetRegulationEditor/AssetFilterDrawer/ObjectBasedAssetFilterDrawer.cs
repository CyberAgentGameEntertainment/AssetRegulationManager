using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetFilterDrawer
{
    [CustomGUIDrawer(typeof(ObjectBasedAssetFilter))]
    internal sealed class ObjectBasedAssetFilterDrawer : GUIDrawer<ObjectBasedAssetFilter>
    {
        private ObjectListablePropertyGUI _listablePropertyGUI;

        public override void Setup(object target)
        {
            base.Setup(target);
            _listablePropertyGUI = new ObjectListablePropertyGUI(ObjectNames.NicifyVariableName(nameof(Target.Object)),
                Target.Object, typeof(Object), false);
        }

        protected override void GUILayout(ObjectBasedAssetFilter target)
        {
            _listablePropertyGUI.DoLayout();
        }
    }
}

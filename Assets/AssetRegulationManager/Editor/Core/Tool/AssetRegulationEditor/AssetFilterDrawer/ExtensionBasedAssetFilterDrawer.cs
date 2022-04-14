using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetFilterDrawer
{
    [CustomGUIDrawer(typeof(ExtensionBasedAssetFilter))]
    internal sealed class ExtensionBasedAssetFilterDrawer : GUIDrawer<ExtensionBasedAssetFilter>
    {
        private TextListablePropertyGUI _listablePropertyGUI;

        public override void Setup(object target)
        {
            base.Setup(target);
            _listablePropertyGUI =
                new TextListablePropertyGUI(ObjectNames.NicifyVariableName(nameof(Target.Extension)),
                    Target.Extension);
        }

        protected override void GUILayout(ExtensionBasedAssetFilter target)
        {
            _listablePropertyGUI.DoLayout();
        }
    }
}

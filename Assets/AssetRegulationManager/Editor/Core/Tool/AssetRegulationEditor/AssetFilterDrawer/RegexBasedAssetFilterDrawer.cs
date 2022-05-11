using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetFilterDrawer
{
    [CustomGUIDrawer(typeof(RegexBasedAssetFilter))]
    internal sealed class RegexBasedAssetFilterDrawer : GUIDrawer<RegexBasedAssetFilter>
    {
        private TextListablePropertyGUI _listablePropertyGUI;

        public override void Setup(object target)
        {
            base.Setup(target);
            _listablePropertyGUI = new TextListablePropertyGUI("Asset Path (Regex)", Target.AssetPathRegex);
        }

        protected override void GUILayout(RegexBasedAssetFilter target)
        {
            target.Condition =
                (AssetFilterCondition)EditorGUILayout.EnumPopup(
                    ObjectNames.NicifyVariableName(nameof(Target.Condition)), target.Condition);
            _listablePropertyGUI.DoLayout();
        }
    }
}

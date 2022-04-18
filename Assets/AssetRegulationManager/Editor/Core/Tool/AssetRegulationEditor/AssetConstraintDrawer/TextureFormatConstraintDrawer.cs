using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(TextureFormatConstraint))]
    internal sealed class TextureFormatConstraintDrawer : GUIDrawer<TextureFormatConstraint>
    {
        private TextureImporterFormatListablePropertyGUI _formatGUI;
        private BuildTargetGroupListablePropertyGUI _targetGUI;

        public override void Setup(object target)
        {
            base.Setup(target);
            _targetGUI = new BuildTargetGroupListablePropertyGUI(ObjectNames.NicifyVariableName(nameof(Target.Target)),
                Target.Target);
            _formatGUI =
                new TextureImporterFormatListablePropertyGUI(ObjectNames.NicifyVariableName(nameof(Target.Format)),
                    Target.Format);
        }

        protected override void GUILayout(TextureFormatConstraint target)
        {
            _targetGUI.DoLayout();
            _formatGUI.DoLayout();
        }

        public sealed class BuildTargetGroupListablePropertyGUI : ListablePropertyGUI<BuildTargetGroup>
        {
            public BuildTargetGroupListablePropertyGUI(string displayName, ListableProperty<BuildTargetGroup> list) :
                base(
                    displayName, list,
                    (rect, label, value) => (BuildTargetGroup)EditorGUI.EnumPopup(rect, new GUIContent(label), value))
            {
            }
        }

        public sealed class TextureImporterFormatListablePropertyGUI : ListablePropertyGUI<TextureImporterFormat>
        {
            public TextureImporterFormatListablePropertyGUI(string displayName,
                ListableProperty<TextureImporterFormat> list) :
                base(
                    displayName, list,
                    (rect, label, value) =>
                        (TextureImporterFormat)EditorGUI.EnumPopup(rect, new GUIContent(label), value))
            {
            }
        }
    }
}

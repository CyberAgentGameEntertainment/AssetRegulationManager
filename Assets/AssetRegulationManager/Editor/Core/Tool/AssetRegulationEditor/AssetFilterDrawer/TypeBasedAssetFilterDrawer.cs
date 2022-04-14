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

    public sealed class TypeReferenceListablePropertyGUI : ListablePropertyGUI<TypeReference>
    {
        private const string TempControlName = "TypeReferenceListablePropertyGUI.TempControl";

        public TypeReferenceListablePropertyGUI(string displayName, ListableProperty<TypeReference> list)
            : base(displayName, list, (rect, label, value, onValueChanged) =>
            {
                var buttonText = value.Name;
                if (string.IsNullOrEmpty(buttonText))
                    buttonText = "-";

                var propertyRect = EditorGUI.PrefixLabel(rect, new GUIContent(label));
                GUI.SetNextControlName(TempControlName);
                if (EditorGUI.DropdownButton(propertyRect, new GUIContent(buttonText), FocusType.Passive))
                {
                    GUI.FocusControl(TempControlName);
                    var dropdown = new TypeSelectDropdown(new AdvancedDropdownState());

                    void OnItemSelected(TypeSelectDropdown.Item item)
                    {
                        value.Name = item.TypeName;
                        value.FullName = item.FullName;
                        value.AssemblyQualifiedName = item.AssemblyQualifiedName;
                        onValueChanged(value);
                        dropdown.OnItemSelected -= OnItemSelected;
                    }

                    dropdown.OnItemSelected += OnItemSelected;
                    dropdown.Show(propertyRect);
                }
            }, () => new TypeReference())
        {
        }
    }
}

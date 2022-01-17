using System;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Foundation.EnabledIfAttributes
{
    [CustomPropertyDrawer(typeof(EnabledIfAttribute))]
    public sealed class EnabledIfAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = (EnabledIfAttribute)attribute;
            var isEnabled = GetIsEnabled(attr, property);

            if (attr.HideMode == HideMode.Disabled)
            {
                GUI.enabled &= isEnabled;
            }

            if (GetIsVisible(attr, property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            if (attr.HideMode == HideMode.Disabled)
            {
                GUI.enabled = true;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var attr = attribute as EnabledIfAttribute;
            return GetIsVisible(attr, property)
                ? EditorGUI.GetPropertyHeight(property)
                : -2;
        }

        private bool GetIsVisible(EnabledIfAttribute attr, SerializedProperty property)
        {
            if (GetIsEnabled(attr, property))
            {
                return true;
            }

            if (attr.HideMode != HideMode.Invisible)
            {
                return true;
            }

            return false;
        }

        private bool GetIsEnabled(EnabledIfAttribute attr, SerializedProperty property)
        {
            return attr.EnableIfValueIs == GetSwitcherPropertyValue(attr, property);
        }

        private int GetSwitcherPropertyValue(EnabledIfAttribute attr, SerializedProperty property)
        {
            var propertyNameIndex = property.propertyPath.LastIndexOf(property.name, StringComparison.Ordinal);
            var switcherPropertyName =
                property.propertyPath.Substring(0, propertyNameIndex) + attr.SwitcherFieldName;
            var switcherProperty = property.serializedObject.FindProperty(switcherPropertyName);
            switch (switcherProperty.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    return switcherProperty.boolValue ? 1 : 0;
                case SerializedPropertyType.Enum:
                    return switcherProperty.intValue;
                case SerializedPropertyType.Generic:
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.Float:
                case SerializedPropertyType.String:
                case SerializedPropertyType.Color:
                case SerializedPropertyType.ObjectReference:
                case SerializedPropertyType.LayerMask:
                case SerializedPropertyType.Vector2:
                case SerializedPropertyType.Vector3:
                case SerializedPropertyType.Vector4:
                case SerializedPropertyType.Rect:
                case SerializedPropertyType.ArraySize:
                case SerializedPropertyType.Character:
                case SerializedPropertyType.AnimationCurve:
                case SerializedPropertyType.Bounds:
                case SerializedPropertyType.Gradient:
                case SerializedPropertyType.Quaternion:
                case SerializedPropertyType.ExposedReference:
                case SerializedPropertyType.FixedBufferSize:
                case SerializedPropertyType.Vector2Int:
                case SerializedPropertyType.Vector3Int:
                case SerializedPropertyType.RectInt:
                case SerializedPropertyType.BoundsInt:
                case SerializedPropertyType.ManagedReference:
                    throw new NotSupportedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEditor;
using UnityEditorInternal;

namespace AssetRegulationManager.Editor.Foundation.ReorderableListUtility
{
    public static class ReorderableListExtensions
    {
        /// <summary>
        ///     If you call this method, ach value of an element is reset to its default value when the element is added.
        /// </summary>
        /// <param name="reorderableList"></param>
        public static void ActivateResetOnAdd(this ReorderableList reorderableList)
        {
            reorderableList.onAddCallback = list =>
            {
                var targetIndex = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                var elementProperty = list.serializedProperty.GetArrayElementAtIndex(targetIndex);
                ResetSerializedProperties(elementProperty);
                list.serializedProperty.serializedObject.ApplyModifiedProperties();
            };
        }

        /// <summary>
        ///     If you call this method, ach value of an element is reset to its default value when the element is added.
        /// </summary>
        /// <param name="reorderableList"></param>
        public static void ActivateResetOnAdd(this ICustomReorderableList reorderableList)
        {
            reorderableList.OnAddCallback = list =>
            {
                var targetIndex = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                var elementProperty = list.serializedProperty.GetArrayElementAtIndex(targetIndex);
                ResetSerializedProperties(elementProperty);
                list.serializedProperty.serializedObject.ApplyModifiedProperties();
            };
        }

        private static void ResetSerializedProperties(SerializedProperty property)
        {
            var depth = property.depth;
            ResetSerializedProperty(property);
            while (property.Next(true))
            {
                if (depth > property.depth)
                {
                    break;
                }

                ResetSerializedProperty(property);
            }
        }

        private static void ResetSerializedProperty(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Integer:
                    property.intValue = default;
                    break;
                case SerializedPropertyType.Boolean:
                    property.boolValue = default;
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = default;
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = default;
                    break;
                case SerializedPropertyType.Color:
                    property.colorValue = default;
                    break;
                case SerializedPropertyType.ObjectReference:
                    property.managedReferenceValue = default;
                    break;
                case SerializedPropertyType.LayerMask:
                    property.intValue = default;
                    break;
                case SerializedPropertyType.Enum:
                    property.enumValueIndex = default;
                    break;
                case SerializedPropertyType.Vector2:
                    property.vector2Value = default;
                    break;
                case SerializedPropertyType.Vector3:
                    property.vector3Value = default;
                    break;
                case SerializedPropertyType.Vector4:
                    property.vector4Value = default;
                    break;
                case SerializedPropertyType.Rect:
                    property.rectValue = default;
                    break;
                case SerializedPropertyType.ArraySize:
                    property.arraySize = default;
                    break;
                case SerializedPropertyType.Character:
                    property.intValue = default;
                    break;
                case SerializedPropertyType.AnimationCurve:
                    property.animationCurveValue = default;
                    break;
                case SerializedPropertyType.Bounds:
                    property.boundsValue = default;
                    break;
                case SerializedPropertyType.Gradient:
                    property.objectReferenceValue = default;
                    break;
                case SerializedPropertyType.Quaternion:
                    property.quaternionValue = default;
                    break;
                case SerializedPropertyType.ExposedReference:
                    property.exposedReferenceValue = default;
                    break;
                case SerializedPropertyType.Vector2Int:
                    property.vector2IntValue = default;
                    break;
                case SerializedPropertyType.Vector3Int:
                    property.vector3IntValue = default;
                    break;
                case SerializedPropertyType.RectInt:
                    property.rectIntValue = default;
                    break;
                case SerializedPropertyType.BoundsInt:
                    property.boundsIntValue = default;
                    break;
                case SerializedPropertyType.ManagedReference:
                    property.managedReferenceValue = default;
                    break;
                case SerializedPropertyType.FixedBufferSize:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

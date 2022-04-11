using System;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(MaxTextureSizeConstraint))]
    internal sealed class MaxTextureSizeConstraintDrawer : GUIDrawer<MaxTextureSizeConstraint>
    {
        protected override void GUILayout(MaxTextureSizeConstraint target)
        {
            target.CountMode =
                (TextureSizeCountMode)EditorGUILayout.EnumPopup(
                    ObjectNames.NicifyVariableName(nameof(target.CountMode)), target.CountMode);

            switch (target.CountMode)
            {
                case TextureSizeCountMode.WidthAndHeight:
                    target.MaxSize =
                        EditorGUILayout.Vector2Field(ObjectNames.NicifyVariableName(nameof(target.MaxSize)),
                            target.MaxSize);
                    break;
                case TextureSizeCountMode.TexelCount:
                    target.MaxTexelCount =
                        EditorGUILayout.IntField(ObjectNames.NicifyVariableName(nameof(target.MaxTexelCount)),
                            target.MaxTexelCount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

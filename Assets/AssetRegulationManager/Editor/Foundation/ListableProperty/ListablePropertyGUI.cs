using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Foundation.ListableProperty
{
    public abstract class ListablePropertyGUI<T>
    {
        private readonly string _displayName;
        private readonly Func<Rect, string, T, T> _drawElement;
        private readonly ListableProperty<T> _list;
        private readonly ListGUI<T> _listGUI;

        protected ListablePropertyGUI(string displayName, ListableProperty<T> list,
            Func<Rect, string, T, T> drawElement)
        {
            _displayName = displayName;
            _list = list;
            _drawElement = drawElement;
            _listGUI = new AnonymousListGUI<T>(list.InternalList, drawElement)
            {
                IndentElements = false,
                ShowTitle = false,
                ShowSize = false
            };
        }

        public void DoLayout()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                if (_list.IsListMode)
                    DrawListMode();
                else
                    DrawSingleMode();
            }
        }

        private void DrawSingleMode()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var titleRect = GetPrefixLabelRect();
                EditorGUI.LabelField(titleRect, _displayName);
                GUILayout.Space(2);

                var fieldRect = EditorGUILayout.GetControlRect(false);
                _list.Value = _drawElement.Invoke(fieldRect, string.Empty, _list.Value);

                _list.IsListMode = GUILayout.Toggle(_list.IsListMode, ListablePropertyEditorUtility.ListIcon,
                    GUI.skin.button, GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.Width(28));
            }
        }

        private void DrawListMode()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var titleRect = GetPrefixLabelRect();
                if (!EditorGUIUtility.hierarchyMode)
                {
                    var num = EditorStyles.foldout.padding.left - EditorStyles.label.padding.left;
                    titleRect.xMin -= num;
                }

                _listGUI.Foldout = EditorGUI.Foldout(titleRect, _listGUI.Foldout, $"{_displayName} List", true);
                GUILayout.Space(2);

                using (var ccs = new EditorGUI.ChangeCheckScope())
                {
                    var count = EditorGUILayout.DelayedIntField(_list.Count);
                    if (ccs.changed)
                        ResizeList(count);
                }

                _list.IsListMode = GUILayout.Toggle(_list.IsListMode, ListablePropertyEditorUtility.ListIcon,
                    GUI.skin.button, GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.Width(28));
            }

            _listGUI.DoLayout();
        }

        private void ResizeList(int newSize)
        {
            while (_list.Count < newSize)
                _list.InternalList.Add(default);

            while (_list.Count > newSize)
                _list.InternalList.RemoveAt(_list.Count - 1);
        }

        private static Rect GetPrefixLabelRect()
        {
            var followingStyle = (GUIStyle)"Button";
            var left = followingStyle.margin.left;
            var width = (float)(EditorGUIUtility.labelWidth - (double)left);
            var rect = GUILayoutUtility.GetRect(width, 18f, followingStyle, GUILayout.ExpandWidth(false));
            rect.xMin += EditorGUI.indentLevel * 15f;
            return rect;
        }
    }

    public sealed class AnonymousListablePropertyGUI<T> : ListablePropertyGUI<T>
    {
        public AnonymousListablePropertyGUI(string displayName, ListableProperty<T> list,
            Func<Rect, string, T, T> drawElement)
            : base(displayName, list, drawElement)
        {
        }
    }

    public sealed class IntListablePropertyGUI : ListablePropertyGUI<int>
    {
        public IntListablePropertyGUI(string displayName, ListableProperty<int> list)
            : base(displayName, list, EditorGUI.IntField)
        {
        }
    }

    public sealed class DelayedIntListablePropertyGUI : ListablePropertyGUI<int>
    {
        public DelayedIntListablePropertyGUI(string displayName, ListableProperty<int> list)
            : base(displayName, list, EditorGUI.DelayedIntField)
        {
        }
    }

    public sealed class FloatListablePropertyGUI : ListablePropertyGUI<float>
    {
        public FloatListablePropertyGUI(string displayName, ListableProperty<float> list)
            : base(displayName, list, EditorGUI.FloatField)
        {
        }
    }

    public sealed class DelayedFloatListablePropertyGUI : ListablePropertyGUI<float>
    {
        public DelayedFloatListablePropertyGUI(string displayName, ListableProperty<float> list)
            : base(displayName, list, EditorGUI.DelayedFloatField)
        {
        }
    }

    public sealed class LongListablePropertyGUI : ListablePropertyGUI<long>
    {
        public LongListablePropertyGUI(string displayName, ListableProperty<long> list)
            : base(displayName, list, EditorGUI.LongField)
        {
        }
    }

    public sealed class DoubleListablePropertyGUI : ListablePropertyGUI<double>
    {
        public DoubleListablePropertyGUI(string displayName, ListableProperty<double> list)
            : base(displayName, list, EditorGUI.DoubleField)
        {
        }
    }

    public sealed class DelayedDoubleListablePropertyGUI : ListablePropertyGUI<double>
    {
        public DelayedDoubleListablePropertyGUI(string displayName, ListableProperty<double> list)
            : base(displayName, list, EditorGUI.DelayedDoubleField)
        {
        }
    }

    public sealed class TextListablePropertyGUI : ListablePropertyGUI<string>
    {
        public TextListablePropertyGUI(string displayName, ListableProperty<string> list)
            : base(displayName, list, EditorGUI.TextField)
        {
        }
    }

    public sealed class DelayedTextListablePropertyGUI : ListablePropertyGUI<string>
    {
        public DelayedTextListablePropertyGUI(string displayName, ListableProperty<string> list)
            : base(displayName, list, EditorGUI.DelayedTextField)
        {
        }
    }

    public sealed class ObjectListablePropertyGUI : ListablePropertyGUI<Object>
    {
        public ObjectListablePropertyGUI(string displayName, ListableProperty<Object> list, Type type,
            bool allowSceneObject) : base(displayName, list,
            (rect, label, value) => EditorGUI.ObjectField(rect, label, value, type, allowSceneObject))
        {
        }
    }

    public sealed class Vector2ListablePropertyGUI : ListablePropertyGUI<Vector2>
    {
        public Vector2ListablePropertyGUI(string displayName, ListableProperty<Vector2> list)
            : base(displayName, list, EditorGUI.Vector2Field)
        {
        }
    }

    public sealed class Vector3ListablePropertyGUI : ListablePropertyGUI<Vector3>
    {
        public Vector3ListablePropertyGUI(string displayName, ListableProperty<Vector3> list)
            : base(displayName, list, EditorGUI.Vector3Field)
        {
        }
    }

    public sealed class Vector4ListablePropertyGUI : ListablePropertyGUI<Vector4>
    {
        public Vector4ListablePropertyGUI(string displayName, ListableProperty<Vector4> list)
            : base(displayName, list, EditorGUI.Vector4Field)
        {
        }
    }

    public sealed class Vector2IntListablePropertyGUI : ListablePropertyGUI<Vector2Int>
    {
        public Vector2IntListablePropertyGUI(string displayName, ListableProperty<Vector2Int> list)
            : base(displayName, list, EditorGUI.Vector2IntField)
        {
        }
    }

    public sealed class Vector3IntListablePropertyGUI : ListablePropertyGUI<Vector3Int>
    {
        public Vector3IntListablePropertyGUI(string displayName, ListableProperty<Vector3Int> list)
            : base(displayName, list, EditorGUI.Vector3IntField)
        {
        }
    }

    public sealed class RectListablePropertyGUI : ListablePropertyGUI<Rect>
    {
        public RectListablePropertyGUI(string displayName, ListableProperty<Rect> list)
            : base(displayName, list, EditorGUI.RectField)
        {
        }
    }

    public sealed class RectIntListablePropertyGUI : ListablePropertyGUI<RectInt>
    {
        public RectIntListablePropertyGUI(string displayName, ListableProperty<RectInt> list)
            : base(displayName, list, EditorGUI.RectIntField)
        {
        }
    }

    public sealed class BoundsListablePropertyGUI : ListablePropertyGUI<Bounds>
    {
        public BoundsListablePropertyGUI(string displayName, ListableProperty<Bounds> list)
            : base(displayName, list, EditorGUI.BoundsField)
        {
        }
    }

    public sealed class BoundsIntListablePropertyGUI : ListablePropertyGUI<BoundsInt>
    {
        public BoundsIntListablePropertyGUI(string displayName, ListableProperty<BoundsInt> list)
            : base(displayName, list, EditorGUI.BoundsIntField)
        {
        }
    }

    public sealed class CurveListablePropertyGUI : ListablePropertyGUI<AnimationCurve>
    {
        public CurveListablePropertyGUI(string displayName, ListableProperty<AnimationCurve> list)
            : base(displayName, list, EditorGUI.CurveField)
        {
        }
    }

    public sealed class ColorListablePropertyGUI : ListablePropertyGUI<Color>
    {
        public ColorListablePropertyGUI(string displayName, ListableProperty<Color> list)
            : base(displayName, list, EditorGUI.ColorField)
        {
        }
    }

    public sealed class GradientListablePropertyGUI : ListablePropertyGUI<Gradient>
    {
        public GradientListablePropertyGUI(string displayName, ListableProperty<Gradient> list)
            : base(displayName, list, EditorGUI.GradientField)
        {
        }
    }

    public sealed class PopupListablePropertyGUI : ListablePropertyGUI<int>
    {
        public PopupListablePropertyGUI(string displayName, ListableProperty<int> list, string[] displayOptions)
            : base(displayName, list, (rect, label, value) => EditorGUI.Popup(rect, label, value, displayOptions))
        {
        }
    }

    public sealed class IntPopupListablePropertyGUI : ListablePropertyGUI<int>
    {
        public IntPopupListablePropertyGUI(string displayName, ListableProperty<int> list, string[] displayOptions,
            int[] optionValues) : base(displayName, list,
            (rect, label, value) => EditorGUI.IntPopup(rect, label, value, displayOptions, optionValues))

        {
        }
    }

    public sealed class EnumPopupListablePropertyGUI : ListablePropertyGUI<Enum>
    {
        public EnumPopupListablePropertyGUI(string displayName, ListableProperty<Enum> list)
            : base(displayName, list, EditorGUI.EnumPopup)
        {
        }
    }

    public sealed class EnumFlagsListablePropertyGUI : ListablePropertyGUI<Enum>
    {
        public EnumFlagsListablePropertyGUI(string displayName, ListableProperty<Enum> list)
            : base(displayName, list, EditorGUI.EnumFlagsField)
        {
        }
    }

    public sealed class LayerListablePropertyGUI : ListablePropertyGUI<int>
    {
        public LayerListablePropertyGUI(string displayName, ListableProperty<int> list)
            : base(displayName, list, EditorGUI.LayerField)
        {
        }
    }

    public sealed class MaskListablePropertyGUI : ListablePropertyGUI<int>
    {
        public MaskListablePropertyGUI(string displayName, ListableProperty<int> list, string[] displayOptions)
            : base(displayName, list, (rect, label, value) => EditorGUI.MaskField(rect, label, value, displayOptions))
        {
        }
    }

    public sealed class TagListablePropertyGUI : ListablePropertyGUI<string>
    {
        public TagListablePropertyGUI(string displayName, ListableProperty<string> list)
            : base(displayName, list, EditorGUI.TagField)
        {
        }
    }
}

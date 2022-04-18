using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Foundation.ListableProperty
{
    public abstract class ListGUI<T>
    {
        private readonly IList<T> _list;

        protected ListGUI(IList<T> list)
        {
            _list = list;
        }

        public bool IndentElements { get; set; } = true;
        public bool ShowTitle { get; set; } = true;
        public bool ShowSize { get; set; } = true;
        public bool Foldout { get; set; }
        public bool CanMoveElements { get; set; } = true;

        public void DoLayout()
        {
            if (ShowTitle)
                DrawTitleField();

            if (!Foldout)
                return;

            if (IndentElements)
                EditorGUI.indentLevel++;

            if (ShowSize) DrawSizeField();

            DrawElementFields();

            if (IndentElements)
                EditorGUI.indentLevel--;
        }

        private void DrawTitleField()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                Foldout = EditorGUILayout.Foldout(Foldout, "Extension", true);
            }
        }

        private void DrawSizeField()
        {
            using (var ccs = new EditorGUI.ChangeCheckScope())
            {
                var size = EditorGUILayout.DelayedIntField("Size", _list.Count);
                if (ccs.changed)
                    ResizeList(size);
            }
        }

        private void DrawElementFields()
        {
            for (var i = 0; i < _list.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    var rect = EditorGUILayout.GetControlRect(true, 18f);
                    _list[i] = DrawElementGUI(rect, $"Element {i}", _list[i]);
                }

                if (Event.current.type == EventType.MouseDown)
                {
                    var rect = GUILayoutUtility.GetLastRect();
                    if (rect.Contains(Event.current.mousePosition) && Event.current.button == 1)
                        OnElementRightClicked(i);
                }
            }
        }

        private void OnElementRightClicked(int index)
        {
            var menu = new GenericMenu();

            // Remove
            menu.AddItem(new GUIContent("Remove"), false, () => _list.RemoveAt(index));

            if (CanMoveElements)
            {
                // Move Up
                var moveUpLabel = new GUIContent("Move Up");
                if (index == 0)
                    menu.AddDisabledItem(moveUpLabel, false);
                else
                    menu.AddItem(moveUpLabel, false, () =>
                    {
                        var targetIndex = index - 1;
                        var item = _list[index];
                        _list.RemoveAt(index);
                        _list.Insert(targetIndex, item);
                    });

                // Move Down
                var moveDownLabel = new GUIContent("Move Down");
                if (index == _list.Count - 1)
                    menu.AddDisabledItem(moveDownLabel, false);
                else
                    menu.AddItem(moveDownLabel, false, () =>
                    {
                        var targetIndex = index + 1;
                        var item = _list[index];
                        _list.RemoveAt(index);
                        _list.Insert(targetIndex, item);
                    });
            }

            menu.ShowAsContext();
        }

        private void ResizeList(int newSize)
        {
            while (_list.Count < newSize)
                _list.Add(default);

            while (_list.Count > newSize)
                _list.RemoveAt(_list.Count - 1);
        }

        protected abstract T DrawElementGUI(Rect rect, string label, T value);
    }

    public sealed class AnonymousListGUI<T> : ListGUI<T>
    {
        private readonly Func<Rect, string, T, T> _drawElementGUI;

        public AnonymousListGUI(IList<T> list, Func<Rect, string, T, T> drawElementGUI) : base(list)
        {
            _drawElementGUI = drawElementGUI;
        }

        protected override T DrawElementGUI(Rect rect, string label, T value)
        {
            return _drawElementGUI.Invoke(rect, label, value);
        }
    }

    public sealed class IntListGUI : ListGUI<int>
    {
        public IntListGUI(IList<int> list) : base(list)
        {
        }

        protected override int DrawElementGUI(Rect rect, string label, int value)
        {
            return EditorGUI.IntField(rect, label, value);
        }
    }

    public sealed class DelayedIntListGUI : ListGUI<int>
    {
        public DelayedIntListGUI(IList<int> list) : base(list)
        {
        }

        protected override int DrawElementGUI(Rect rect, string label, int value)
        {
            return EditorGUI.DelayedIntField(rect, label, value);
        }
    }

    public sealed class FloatListGUI : ListGUI<float>
    {
        public FloatListGUI(IList<float> list) : base(list)
        {
        }

        protected override float DrawElementGUI(Rect rect, string label, float value)
        {
            return EditorGUI.FloatField(rect, label, value);
        }
    }

    public sealed class DelayedFloatListGUI : ListGUI<float>
    {
        public DelayedFloatListGUI(IList<float> list) : base(list)
        {
        }

        protected override float DrawElementGUI(Rect rect, string label, float value)
        {
            return EditorGUI.DelayedFloatField(rect, label, value);
        }
    }

    public sealed class LongListGUI : ListGUI<long>
    {
        public LongListGUI(IList<long> list) : base(list)
        {
        }

        protected override long DrawElementGUI(Rect rect, string label, long value)
        {
            return EditorGUI.LongField(rect, label, value);
        }
    }

    public sealed class DoubleListGUI : ListGUI<double>
    {
        public DoubleListGUI(IList<double> list) : base(list)
        {
        }

        protected override double DrawElementGUI(Rect rect, string label, double value)
        {
            return EditorGUI.DoubleField(rect, label, value);
        }
    }

    public sealed class DelayedDoubleListGUI : ListGUI<double>
    {
        public DelayedDoubleListGUI(IList<double> list) : base(list)
        {
        }

        protected override double DrawElementGUI(Rect rect, string label, double value)
        {
            return EditorGUI.DelayedDoubleField(rect, label, value);
        }
    }

    public sealed class TextListGUI : ListGUI<string>
    {
        public TextListGUI(IList<string> list) : base(list)
        {
        }

        protected override string DrawElementGUI(Rect rect, string label, string value)
        {
            return EditorGUI.TextField(rect, label, value);
        }
    }

    public sealed class DelayedTextListGUI : ListGUI<string>
    {
        public DelayedTextListGUI(IList<string> list) : base(list)
        {
        }

        protected override string DrawElementGUI(Rect rect, string label, string value)
        {
            return EditorGUI.DelayedTextField(rect, label, value);
        }
    }

    public sealed class ObjectListGUI : ListGUI<Object>
    {
        private readonly bool _allowSceneObject;
        private readonly Type _type;

        public ObjectListGUI(IList<Object> list, Type type, bool allowSceneObject) : base(list)
        {
            _type = type;
            _allowSceneObject = allowSceneObject;
        }

        protected override Object DrawElementGUI(Rect rect, string label, Object value)
        {
            return EditorGUI.ObjectField(rect, label, value, _type, _allowSceneObject);
        }
    }

    public sealed class Vector2ListGUI : ListGUI<Vector2>
    {
        public Vector2ListGUI(IList<Vector2> list) : base(list)
        {
        }

        protected override Vector2 DrawElementGUI(Rect rect, string label, Vector2 value)
        {
            return EditorGUI.Vector2Field(rect, label, value);
        }
    }

    public sealed class Vector3ListGUI : ListGUI<Vector3>
    {
        public Vector3ListGUI(IList<Vector3> list) : base(list)
        {
        }

        protected override Vector3 DrawElementGUI(Rect rect, string label, Vector3 value)
        {
            return EditorGUI.Vector3Field(rect, label, value);
        }
    }

    public sealed class Vector4ListGUI : ListGUI<Vector4>
    {
        public Vector4ListGUI(IList<Vector4> list) : base(list)
        {
        }

        protected override Vector4 DrawElementGUI(Rect rect, string label, Vector4 value)
        {
            return EditorGUI.Vector4Field(rect, label, value);
        }
    }

    public sealed class Vector2IntListGUI : ListGUI<Vector2Int>
    {
        public Vector2IntListGUI(IList<Vector2Int> list) : base(list)
        {
        }

        protected override Vector2Int DrawElementGUI(Rect rect, string label, Vector2Int value)
        {
            return EditorGUI.Vector2IntField(rect, label, value);
        }
    }

    public sealed class Vector3IntListGUI : ListGUI<Vector3Int>
    {
        public Vector3IntListGUI(IList<Vector3Int> list) : base(list)
        {
        }

        protected override Vector3Int DrawElementGUI(Rect rect, string label, Vector3Int value)
        {
            return EditorGUI.Vector3IntField(rect, label, value);
        }
    }

    public sealed class RectListGUI : ListGUI<Rect>
    {
        public RectListGUI(IList<Rect> list) : base(list)
        {
        }

        protected override Rect DrawElementGUI(Rect rect, string label, Rect value)
        {
            return EditorGUI.RectField(rect, label, value);
        }
    }

    public sealed class RectIntListGUI : ListGUI<RectInt>
    {
        public RectIntListGUI(IList<RectInt> list) : base(list)
        {
        }

        protected override RectInt DrawElementGUI(Rect rect, string label, RectInt value)
        {
            return EditorGUI.RectIntField(rect, label, value);
        }
    }

    public sealed class BoundsListGUI : ListGUI<Bounds>
    {
        public BoundsListGUI(IList<Bounds> list) : base(list)
        {
        }

        protected override Bounds DrawElementGUI(Rect rect, string label, Bounds value)
        {
            return EditorGUI.BoundsField(rect, label, value);
        }
    }

    public sealed class BoundsIntListGUI : ListGUI<BoundsInt>
    {
        public BoundsIntListGUI(IList<BoundsInt> list) : base(list)
        {
        }

        protected override BoundsInt DrawElementGUI(Rect rect, string label, BoundsInt value)
        {
            return EditorGUI.BoundsIntField(rect, label, value);
        }
    }

    public sealed class CurveListGUI : ListGUI<AnimationCurve>
    {
        public CurveListGUI(IList<AnimationCurve> list) : base(list)
        {
        }

        protected override AnimationCurve DrawElementGUI(Rect rect, string label, AnimationCurve value)
        {
            return EditorGUI.CurveField(rect, label, value);
        }
    }

    public sealed class ColorListGUI : ListGUI<Color>
    {
        public ColorListGUI(IList<Color> list) : base(list)
        {
        }

        protected override Color DrawElementGUI(Rect rect, string label, Color value)
        {
            return EditorGUI.ColorField(rect, label, value);
        }
    }

    public sealed class GradientListGUI : ListGUI<Gradient>
    {
        public GradientListGUI(IList<Gradient> list) : base(list)
        {
        }

        protected override Gradient DrawElementGUI(Rect rect, string label, Gradient value)
        {
            return EditorGUI.GradientField(rect, label, value);
        }
    }

    public sealed class PopupListGUI : ListGUI<int>
    {
        private readonly string[] _displayOptions;

        public PopupListGUI(IList<int> list, string[] displayOptions) : base(list)
        {
            _displayOptions = displayOptions;
        }

        protected override int DrawElementGUI(Rect rect, string label, int value)
        {
            return EditorGUI.Popup(rect, label, value, _displayOptions);
        }
    }

    public sealed class IntPopupListGUI : ListGUI<int>
    {
        private readonly string[] _displayOptions;
        private readonly int[] _optionValues;

        public IntPopupListGUI(IList<int> list, string[] displayOptions, int[] optionValues) : base(list)
        {
            _displayOptions = displayOptions;
            _optionValues = optionValues;
        }

        protected override int DrawElementGUI(Rect rect, string label, int value)
        {
            return EditorGUI.IntPopup(rect, label, value, _displayOptions, _optionValues);
        }
    }

    public sealed class EnumPopupListGUI : ListGUI<Enum>
    {
        public EnumPopupListGUI(IList<Enum> list) : base(list)
        {
        }

        protected override Enum DrawElementGUI(Rect rect, string label, Enum value)
        {
            return EditorGUI.EnumPopup(rect, label, value);
        }
    }

    public sealed class EnumFlagsListGUI : ListGUI<Enum>
    {
        public EnumFlagsListGUI(IList<Enum> list) : base(list)
        {
        }

        protected override Enum DrawElementGUI(Rect rect, string label, Enum value)
        {
            return EditorGUI.EnumFlagsField(rect, label, value);
        }
    }

    public sealed class LayerListGUI : ListGUI<int>
    {
        public LayerListGUI(IList<int> list) : base(list)
        {
        }

        protected override int DrawElementGUI(Rect rect, string label, int value)
        {
            return EditorGUI.LayerField(rect, label, value);
        }
    }

    public sealed class MaskListGUI : ListGUI<int>
    {
        private readonly string[] _displayOptions;

        public MaskListGUI(IList<int> list, string[] displayOptions) : base(list)
        {
            _displayOptions = displayOptions;
        }

        protected override int DrawElementGUI(Rect rect, string label, int value)
        {
            return EditorGUI.MaskField(rect, label, value, _displayOptions);
        }
    }

    public sealed class TagListGUI : ListGUI<string>
    {
        public TagListGUI(IList<string> list) : base(list)
        {
        }

        protected override string DrawElementGUI(Rect rect, string label, string value)
        {
            return EditorGUI.TagField(rect, label, value);
        }
    }
}

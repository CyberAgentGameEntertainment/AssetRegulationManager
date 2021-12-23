// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRegulationManager.Editor.Foundation.ListableProperty
{
    /// <summary>
    ///     <para>Property that can be treated as a single value or as an array.</para>
    ///     <para>
    ///         If you serialize this property, it will appear in the Inspector as a non-array property.
    ///         If you want to treat it as array, click the right button of the property to switch it to an array.
    ///     </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ListableProperty<T>
    {
        [SerializeField] private bool _isListMode;
        [SerializeField] private List<T> _values = new List<T>();

        public ListableProperty(bool isListMode = false)
        {
            _isListMode = isListMode;
        }

        /// <summary>
        ///     If true, you can use this property as an array.
        /// </summary>
        public bool IsListMode
        {
            get => _isListMode;
            set => _isListMode = value;
        }

        /// <summary>
        ///     Get all values.
        ///     If <see cref="IsListMode" /> is false, returns only the first element of the values.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetValues()
        {
            if (_isListMode && _values.Count >= 1)
            {
                yield return _values[0];
            }

            foreach (var value in _values)
            {
                yield return value;
            }
        }

        /// <summary>
        ///     Set a value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if <see cref="IsListMode" /> is false and you attempt to set a value
        ///     with an index higher than or equal to 1.
        /// </exception>
        public void SetValue(T value, int index = -1)
        {
            if (index == -1 || index == 0)
            {
                _values[0] = value;
                return;
            }

            if (!_isListMode)
            {
                throw new InvalidOperationException("Not in list mode, you have to specify zer or one for index.");
            }

            _values[index] = value;
        }

        public void RemoveValue(T value)
        {
            var index = _values.IndexOf(value);
            RemoveValue(index);
        }

        /// <summary>
        ///     Remove a value.
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if <see cref="IsListMode" /> is false and you attempt to remove a value
        ///     with an index higher than or equal to 1.
        /// </exception>
        public void RemoveValue(int index = -1)
        {
            if (index == -1 || index == 0)
            {
                _values.RemoveAt(0);
                return;
            }

            if (!_isListMode)
            {
                throw new InvalidOperationException("Not in list mode, you have to specify zer or one for index.");
            }

            _values.RemoveAt(index);
        }
    }
}

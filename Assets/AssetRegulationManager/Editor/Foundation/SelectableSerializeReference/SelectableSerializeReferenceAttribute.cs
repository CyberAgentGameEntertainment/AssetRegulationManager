// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEngine;

namespace AssetRegulationManager.Editor.Foundation.SelectableSerializeReference
{
    /// <summary>
    ///     if you add this attribute together with <see cref="SerializeReference" />, you can select derived classes from
    ///     the Inspector and inject it's instance.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class SelectableSerializeReferenceAttribute : PropertyAttribute
    {
        /// <summary>
        /// </summary>
        /// <param name="useClassNameToLabel">If true, the class name will be used for the label in the Inspector.</param>
        public SelectableSerializeReferenceAttribute(bool useClassNameToLabel = false)
        {
            UseClassNameToLabel = useClassNameToLabel;
        }

        public bool UseClassNameToLabel { get; }
    }
}

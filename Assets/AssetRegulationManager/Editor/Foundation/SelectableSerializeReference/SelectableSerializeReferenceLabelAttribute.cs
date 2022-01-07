// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEngine;

namespace AssetRegulationManager.Editor.Foundation.SelectableSerializeReference
{
    /// <summary>
    ///     if you can add this attribute to the class represented by the type of the field with
    ///     <see cref="SerializeReference" /> and <see cref="SelectableSerializeReferenceAttribute" />, you can override the
    ///     display name in Inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class SelectableSerializeReferenceLabelAttribute : PropertyAttribute
    {
        public SelectableSerializeReferenceLabelAttribute(string label)
        {
            Label = label;
        }

        public string Label { get; }
    }
}

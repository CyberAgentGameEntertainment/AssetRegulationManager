// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
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
        public SelectableSerializeReferenceAttribute(LabelType labelType = LabelType.Default)
        {
            LabelType = labelType;
        }

        public LabelType LabelType { get; }
    }
}

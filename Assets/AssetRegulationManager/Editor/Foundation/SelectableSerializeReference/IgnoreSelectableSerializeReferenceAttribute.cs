// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEngine;

namespace AssetRegulationManager.Editor.Foundation.SelectableSerializeReference
{
    /// <summary>
    ///     Classes with this attribute will be excluded from the target of <see cref="SelectableSerializeReference" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class IgnoreSelectableSerializeReferenceAttribute : PropertyAttribute
    {
    }
}

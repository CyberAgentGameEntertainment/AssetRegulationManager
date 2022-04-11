using System;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class IgnoreAssetFilterAttribute : PropertyAttribute
    {
    }
}

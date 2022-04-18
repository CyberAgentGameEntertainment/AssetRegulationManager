using System;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AssetConstraintAttribute : PropertyAttribute
    {
        public AssetConstraintAttribute(string menuName, string displayName)
        {
            MenuName = menuName;
            DisplayName = displayName;
        }

        public string MenuName { get; }
        public string DisplayName { get; }
    }
}

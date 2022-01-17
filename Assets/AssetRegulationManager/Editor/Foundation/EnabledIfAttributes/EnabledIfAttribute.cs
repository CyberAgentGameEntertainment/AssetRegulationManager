using System;
using UnityEngine;

namespace AssetRegulationManager.Editor.Foundation.EnabledIfAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EnabledIfAttribute : PropertyAttribute
    {
        public EnabledIfAttribute(string switcherFieldName, bool enableIfValueIs, HideMode hideMode = HideMode.Disabled)
            : this(switcherFieldName, enableIfValueIs ? 1 : 0, hideMode)
        {
        }

        public EnabledIfAttribute(string switcherFieldName, int enableIfValueIs, HideMode hideMode = HideMode.Disabled)
        {
            HideMode = hideMode;
            SwitcherFieldName = switcherFieldName;
            EnableIfValueIs = enableIfValueIs;
        }

        public int EnableIfValueIs { get; }
        public HideMode HideMode { get; }
        public string SwitcherFieldName { get; }
    }
}

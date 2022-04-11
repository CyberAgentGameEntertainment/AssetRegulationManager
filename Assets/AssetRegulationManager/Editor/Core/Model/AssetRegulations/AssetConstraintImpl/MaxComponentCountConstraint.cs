// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    /// <summary>
    ///     Base class of the Limitation of the maximum number of components attached to a GameObject.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    [Serializable]
    public abstract class MaxComponentCountConstraint<TComponent> : AssetConstraint<GameObject>
        where TComponent : Component
    {
        [SerializeField] private int _maxCount;
        [SerializeField] private bool _excludeInactive;
        
        private int _latestValue;

        public int MaxCount
        {
            get => _maxCount;
            set => _maxCount = value;
        }

        public bool ExcludeInactive
        {
            get => _excludeInactive;
            set => _excludeInactive = value;
        }

        public override string GetDescription()
        {
            var name = ObjectNames.NicifyVariableName(typeof(TComponent).Name);
            var desc = $"Max {name} Count: {_maxCount} ({(_excludeInactive ? "Exclude" : "Include")} Inactive)";
            return desc;
        }

        public override string GetLatestValueAsText()
        {
            return _latestValue.ToString();
        }

        protected override bool CheckInternal(GameObject asset)
        {
            Assert.IsNotNull(asset);

            var count = asset.GetComponentsInChildren<TComponent>(!_excludeInactive).Length;
            _latestValue = count;
            return count <= _maxCount;
        }
    }
}

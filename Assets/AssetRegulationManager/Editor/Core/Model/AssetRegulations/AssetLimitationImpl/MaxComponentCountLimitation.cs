// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl
{
    [Serializable]
    public abstract class MaxComponentCountLimitation<TComponent> : AssetLimitation<GameObject>
        where TComponent : Component
    {
        [SerializeField] private int _maxCount;

        public int MaxCount
        {
            set => _maxCount = value;
            get => _maxCount;
        }

        public override string GetDescription()
        {
            var name = ObjectNames.NicifyVariableName(typeof(TComponent).Name);
            var desc = $"Max {name} Count: {_maxCount}";
            return desc;
        }

        protected override bool CheckInternal(GameObject asset)
        {
            Assert.IsNotNull(asset);

            var count = asset.GetComponentsInChildren<TComponent>().Length;
            return count <= _maxCount;
        }
    }
}

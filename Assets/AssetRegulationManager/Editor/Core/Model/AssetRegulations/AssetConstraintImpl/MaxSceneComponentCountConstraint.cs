// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    /// <summary>
    ///     Base class of the Limitation of the maximum number of components in a Scene.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    [Serializable]
    public abstract class MaxSceneComponentCountConstraint<TComponent> : AssetConstraint<SceneAsset>
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
            var desc =
                $"Max {name} Count in Scene: {_maxCount} ({(_excludeInactive ? "Exclude" : "Include")} Inactive)";
            return desc;
        }

        public override string GetLatestValueAsText()
        {
            return _latestValue.ToString();
        }

        protected override bool CheckInternal(SceneAsset asset)
        {
            Assert.IsNotNull(asset);

            var sceneAssetPath = AssetDatabase.GetAssetPath(asset);
            if (!AssetConstraintUtility.OpenScene(sceneAssetPath))
            {
                throw new Exception("The process was canceled by user operation.");
            }

            var count = AssetConstraintUtility.GetAllComponentsInActiveScene<TComponent>(!_excludeInactive).Count();
            _latestValue = count;
            return count <= _maxCount;
        }
    }
}

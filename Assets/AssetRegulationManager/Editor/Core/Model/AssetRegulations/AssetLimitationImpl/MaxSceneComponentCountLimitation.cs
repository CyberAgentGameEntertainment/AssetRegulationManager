// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl
{
    /// <summary>
    ///     Base class of the Limitation of the maximum number of components in a Scene.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    [Serializable]
    public abstract class MaxSceneComponentCountLimitation<TComponent> : AssetLimitation<SceneAsset>
        where TComponent : Component
    {
        [SerializeField] private int _maxCount;
        private int _latestValue;

        public int MaxCount
        {
            get => _maxCount;
            set => _maxCount = value;
        }

        public override string GetDescription()
        {
            var name = ObjectNames.NicifyVariableName(typeof(TComponent).Name);
            var desc = $"Max {name} Count in Scene: {_maxCount}";
            return desc;
        }

        public override string GetLatestValueAsText()
        {
            return _latestValue.ToString();
        }

        protected override bool CheckInternal(SceneAsset asset)
        {
            Assert.IsNotNull(asset);

            var isDirty = false;
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.isDirty)
                {
                    isDirty = true;
                    break;
                }
            }

            if (isDirty)
            {
                if (EditorUtility.DisplayDialog("Scene(s) Have Been Modified",
                        "Do you want to save the changes you made in the scenes?", "Save", "Cancel"))
                {
                    EditorSceneManager.SaveOpenScenes();
                }
                else
                {
                    throw new Exception("The process was canceled by user operation.");
                }
            }

            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(asset));

            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            var count = rootGameObjects.SelectMany(x => x.GetComponentsInChildren<TComponent>()).Count();
            _latestValue = count;
            return count <= _maxCount;
        }
    }
}

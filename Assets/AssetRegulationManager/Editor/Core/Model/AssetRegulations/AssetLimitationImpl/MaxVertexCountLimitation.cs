// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Foundation.SelectableSerializeReference;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl
{
    [Serializable]
    [SelectableSerializeReferenceLabel("Max Vertex Count (GameObject, Mesh)")]
    public sealed class MaxVertexCountLimitation : AssetLimitation<Object>
    {
        [SerializeField] private int _maxCount;
        [SerializeField] private bool _excludeChildren;
        [SerializeField] private bool _excludeInactive;
        [SerializeField] private bool _allowDuplicateCount;
        private int _latestValue;

        public int MaxCount
        {
            get => _maxCount;
            set => _maxCount = value;
        }

        public bool ExcludeChildren
        {
            get => _excludeChildren;
            set => _excludeChildren = value;
        }

        public bool AllowDuplicateCount
        {
            get => _allowDuplicateCount;
            set => _allowDuplicateCount = value;
        }

        public override string GetDescription()
        {
            var desc = $"Max Vertex Count: {_maxCount}";
            if (_excludeChildren)
            {
                desc = $"{desc} (Include Children)";
            }

            return desc;
        }

        public override string GetLatestValueAsText()
        {
            return _latestValue.ToString();
        }

        protected override bool CheckInternal(Object asset)
        {
            Assert.IsNotNull(asset);
            var meshes = new List<Mesh>();

            // If the asset is FBX, Prefab, etc., get meshes from MeshFilter or SkinnedMeshRenderer.
            if (asset is GameObject gameObj)
            {
                meshes.AddRange(
                    AssetLimitationUtility.GetMeshesFromGameObject(gameObj, !_excludeChildren, !_excludeInactive));
            }

            // If the asset is Mesh, add it.
            if (asset is Mesh mesh)
            {
                meshes.Add(mesh);
            }

            if (!_allowDuplicateCount)
            {
                meshes = meshes.Distinct().ToList();
            }

            var vertexCount = meshes.Aggregate(0, (x, m) => x + m.vertexCount);
            _latestValue = vertexCount;
            return vertexCount <= _maxCount;
        }
    }
}

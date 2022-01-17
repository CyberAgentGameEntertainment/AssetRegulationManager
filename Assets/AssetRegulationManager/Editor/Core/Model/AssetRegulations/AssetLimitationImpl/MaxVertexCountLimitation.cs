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
    [SelectableSerializeReferenceLabel("Max Vertex Count")]
    public sealed class MaxVertexCountLimitation : AssetLimitation<Object>
    {
        [SerializeField] private int _maxCount;
        [SerializeField] private bool _includeChildren;
        private int _latestValue;

        public int MaxCount
        {
            get => _maxCount;
            set => _maxCount = value;
        }

        public bool IncludeChildren
        {
            get => _includeChildren;
            set => _includeChildren = value;
        }

        public override string GetDescription()
        {
            var desc = $"Max Vertex Count: {_maxCount}";
            if (_includeChildren)
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
                if (_includeChildren)
                {
                    foreach (var meshFilter in gameObj.GetComponentsInChildren<MeshFilter>())
                    {
                        meshes.Add(meshFilter.sharedMesh);
                    }

                    foreach (var skinnedMeshRenderer in gameObj.GetComponentsInChildren<SkinnedMeshRenderer>())
                    {
                        meshes.Add(skinnedMeshRenderer.sharedMesh);
                    }
                }
                else
                {
                    if (gameObj.TryGetComponent<MeshFilter>(out var meshFilter))
                    {
                        meshes.Add(meshFilter.sharedMesh);
                    }

                    if (gameObj.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
                    {
                        meshes.Add(skinnedMeshRenderer.sharedMesh);
                    }
                }
            }

            // If the asset is Mesh, add it.
            if (asset is Mesh mesh)
            {
                meshes.Add(mesh);
            }

            var vertexCount = meshes.Aggregate(0, (i, m) => i + m.vertexCount);
            _latestValue = vertexCount;
            return vertexCount <= _maxCount;
        }
    }
}

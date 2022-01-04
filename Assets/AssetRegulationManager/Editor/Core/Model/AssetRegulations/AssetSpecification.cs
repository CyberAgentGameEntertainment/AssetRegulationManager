// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using AssetRegulationManager.Editor.Foundation.SelectableSerializeReference;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    [Serializable]
    public sealed class AssetSpecification
    {
        [SerializeReference] [SelectableSerializeReference(true)]
        private List<IAssetLimitation> _limitations = new List<IAssetLimitation>();

        public IReadOnlyList<IAssetLimitation> Limitations => _limitations;

        public bool Meet(Object obj)
        {
            foreach (var limitation in _limitations)
            {
                if (!limitation.Check(obj))
                {
                    return false;
                }
            }

            return true;
        }

        public string GetDescription()
        {
            var result = new StringBuilder();
            var isFirstItem = true;
            foreach (var limitation in _limitations)
            {
                if (limitation == null)
                {
                    continue;
                }

                var description = limitation.GetDescription();
                if (string.IsNullOrEmpty(description))
                {
                    continue;
                }

                if (!isFirstItem)
                {
                    result.Append(", ");
                }

                result.Append(description);
                isFirstItem = false;
            }

            return result.Length >= 1 ? result.ToString() : "None";
        }
    }
}

using System;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    [Serializable]
    [AssetConstraint("File/Asset Type", "Asset Type")]
    public sealed class AssetTypeConstraint : AssetConstraint<Object>
    {
        [SerializeField] private TypeReferenceListableProperty _type = new TypeReferenceListableProperty();
        [SerializeField] private bool _matchWithDerivedTypes = true;

        private string _latestValue;

        /// <summary>
        ///     Type.
        /// </summary>
        public TypeReferenceListableProperty Type => _type;

        public bool MatchWithDerivedTypes
        {
            get => _matchWithDerivedTypes;
            set => _matchWithDerivedTypes = value;
        }

        public override string GetDescription()
        {
            var types = new StringBuilder();
            var typeCount = 0;
            foreach (var type in _type)
            {
                if (type == null)
                    continue;

                if (typeCount >= 1) types.Append(" || ");

                types.Append(type.Name);
                typeCount++;
            }

            if (typeCount >= 2)
            {
                types.Insert(0, "( ");
                types.Append(" )");
            }

            var result = $"Type: {types}";
            if (MatchWithDerivedTypes)
                result += " and derived types";
            return result;
        }

        public override string GetLatestValueAsText()
        {
            return string.IsNullOrEmpty(_latestValue) ? "None" : _latestValue;
        }

        /// <inheritdoc />
        protected override bool CheckInternal(Object asset)
        {
            Assert.IsNotNull(asset);

            var assetType = asset.GetType();
            _latestValue = assetType.ToString();

            foreach (var typeRef in _type)
            {
                if (typeRef == null)
                    continue;

                var type = System.Type.GetType(typeRef.AssemblyQualifiedName);

                if (type == null)
                    continue;

                if (type == assetType)
                    return true;

                if (_matchWithDerivedTypes && assetType.IsSubclassOf(type))
                    return true;
            }

            return false;
        }
    }
}

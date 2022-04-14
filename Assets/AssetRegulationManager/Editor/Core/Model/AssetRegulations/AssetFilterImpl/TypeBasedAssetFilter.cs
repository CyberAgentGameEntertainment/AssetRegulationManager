using System;
using System.Collections.Generic;
using System.Text;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl
{
    [Serializable]
    public class TypeReference
    {
        [SerializeField] private string _name;
        [SerializeField] private string _fullName;
        [SerializeField] private string _assemblyQualifiedName;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string FullName
        {
            get => _fullName;
            set => _fullName = value;
        }

        public string AssemblyQualifiedName
        {
            get => _assemblyQualifiedName;
            set => _assemblyQualifiedName = value;
        }
    }

    [Serializable]
    public sealed class TypeReferenceListableProperty : ListableProperty<TypeReference>
    {
    }

    /// <summary>
    ///     Filter to pass assets if matches the type.
    /// </summary>
    [Serializable]
    [AssetFilter("Type Filter", "Type Filter")]
    public sealed class TypeBasedAssetFilter : AssetFilterBase
    {
        [SerializeField] private TypeReferenceListableProperty _type = new TypeReferenceListableProperty();
        [SerializeField] private bool _matchWithDerivedTypes = true;
        private List<Type> _types = new List<Type>();

        /// <summary>
        ///     Type.
        /// </summary>
        public TypeReferenceListableProperty Type => _type;

        public bool MatchWithDerivedTypes
        {
            get => _matchWithDerivedTypes;
            set => _matchWithDerivedTypes = value;
        }

        public override void SetupForMatching()
        {
            _types.Clear();
            foreach (var typeRef in _type)
            {
                if (typeRef == null)
                    continue;

                var type = System.Type.GetType(typeRef.AssemblyQualifiedName);
                _types.Add(type);
            }
        }

        /// <inheritdoc />
        public override bool IsMatch(string assetPath, Type assetType)
        {
            if (assetType == null)
                return false;


            foreach (var type in _types)
            {
                if (type == assetType)
                    return true;

                if (_matchWithDerivedTypes && assetType.IsSubclassOf(type))
                    return true;
            }

            return false;
        }

        public override string GetDescription()
        {
            var result = new StringBuilder();
            var elementCount = 0;
            foreach (var type in _type)
            {
                if (type == null || string.IsNullOrEmpty(type.Name))
                    continue;

                if (elementCount >= 1)
                    result.Append(" || ");

                result.Append(type.Name);
                elementCount++;
            }

            if (result.Length >= 1)
            {
                if (elementCount >= 2)
                {
                    result.Insert(0, "( ");
                    result.Append(" )");
                }

                result.Insert(0, "Type: ");
            }

            return result.ToString();
        }
    }
}

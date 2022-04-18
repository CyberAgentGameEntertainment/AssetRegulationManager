using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssetRegulationManager.Editor.Core.Shared;
using AssetRegulationManager.Editor.Foundation.OrderCollections;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    [Serializable]
    public sealed class AssetRegulation
    {
        [SerializeField] private string _id;

        [SerializeField] private StringObservableProperty _name = new StringObservableProperty("New Asset Regulation");

        [SerializeField] private ObservableAssetGroupDictionary _assetGroups = new ObservableAssetGroupDictionary();

        [SerializeField]
        private ObservableAssetLimitationDictionary _constraints = new ObservableAssetLimitationDictionary();

        [SerializeField] private StringOrderCollection _assetGroupOrders = new StringOrderCollection();

        [SerializeField] private StringOrderCollection _constraintOrders = new StringOrderCollection();

        private readonly Subject<(string id, int index)> _constraintOrderChangedSubject =
            new Subject<(string id, int index)>();

        private readonly ObservableProperty<string> _constraintsDescription = new ObservableProperty<string>();

        private readonly Subject<(string id, int index)> _assetGroupOrderChangedSubject =
            new Subject<(string id, int index)>();

        private readonly ObservableProperty<string> _assetGroupsDescription = new ObservableProperty<string>();

        public AssetRegulation()
        {
            _id = IdentifierFactory.Create();
        }

        public IObservable<(string id, int index)> AssetGroupOrderChangedAsObservable => _assetGroupOrderChangedSubject;

        public IObservable<(string id, int index)> ConstraintOrderChangedAsObservable => _constraintOrderChangedSubject;

        public string Id => _id;

        public ObservableProperty<string> Name => _name;

        public IReadOnlyObservableDictionary<string, AssetGroup> AssetGroups => _assetGroups;

        public IReadOnlyObservableProperty<string> AssetGroupsDescription => _assetGroupsDescription;

        public IReadOnlyObservableDictionary<string, IAssetConstraint> Constraints => _constraints;

        public IReadOnlyObservableProperty<string> ConstraintsDescription => _constraintsDescription;

        public AssetGroup AddAssetGroup()
        {
            var group = new AssetGroup();
            AddAssetGroup(group);
            return group;
        }

        internal void AddAssetGroup(AssetGroup group)
        {
            _assetGroupOrders.Add(group.Id);
            _assetGroups.Add(group.Id, group);
        }

        public void RemoveAssetGroup(string id)
        {
            _assetGroups.Remove(id);
            _assetGroupOrders.Remove(id);
        }

        public int GetAssetGroupOrder(string id)
        {
            return _assetGroupOrders.GetIndex(id);
        }

        public void SetAssetGroupOrder(string id, int index)
        {
            _assetGroupOrders.SetIndex(id, index);
            _assetGroupOrderChangedSubject.OnNext((id, index));
        }

        public void RefreshTargetsDescription()
        {
            var groupDescriptions = new List<string>();

            foreach (var group in _assetGroups.Values.OrderBy(x => _assetGroupOrders.GetIndex(x.Id)))
            {
                var groupDescription = group.GetDescription();

                if (string.IsNullOrEmpty(groupDescription))
                    continue;

                groupDescriptions.Add(groupDescription);
            }

            if (groupDescriptions.Count == 0)
            {
                _assetGroupsDescription.Value = "(None)";
                return;
            }

            var description = new StringBuilder();
            for (var i = 0; i < groupDescriptions.Count; i++)
            {
                var groupDescription = groupDescriptions[i];

                if (i >= 1)
                    description.Append(" || ");

                if (groupDescriptions.Count >= 2)
                    description.Append(" (");

                description.Append(groupDescription);

                if (groupDescriptions.Count >= 2)
                    description.Append(") ");
            }

            _assetGroupsDescription.Value = description.ToString();
        }

        public void Setup()
        {
            foreach (var group in _assetGroups.Values)
                group.Setup();
        }

        public bool IsTargetAsset(string assetPath, Type assetType)
        {
            foreach (var group in _assetGroups.Values.OrderBy(x => _assetGroupOrders.GetIndex(x.Id)))
                if (group.Contains(assetPath, assetType))
                    return true;

            return false;
        }

        public T AddConstraint<T>() where T : IAssetConstraint, new()
        {
            var assetConstraint = new T();
            AddConstraint(assetConstraint);
            return assetConstraint;
        }

        public IAssetConstraint AddConstraint(Type type)
        {
            var constraint = (IAssetConstraint)Activator.CreateInstance(type);
            AddConstraint(constraint);
            return constraint;
        }

        public void AddConstraint<T>(T constraint) where T : IAssetConstraint
        {
            _constraintOrders.Add(constraint.Id);
            _constraints.Add(constraint.Id, constraint);
        }

        public void RemoveConstraint(string id)
        {
            _constraints.Remove(id);
            _constraintOrders.Remove(id);
        }

        public int GetConstraintOrder(string id)
        {
            return _constraintOrders.GetIndex(id);
        }

        public void SetConstraintOrder(string id, int index)
        {
            _constraintOrders.SetIndex(id, index);
            _constraintOrderChangedSubject.OnNext((id, index));
        }

        public bool CheckConstraint(Object obj)
        {
            foreach (var regulation in _constraints.Values.OrderBy(x => _constraintOrders.GetIndex(x.Id)))
                if (!regulation.Check(obj))
                    return false;

            return true;
        }

        public void RefreshConstraintDescription()
        {
            var result = new StringBuilder();
            var isFirstItem = true;

            foreach (var regulation in _constraints.Values.OrderBy(x => _constraintOrders.GetIndex(x.Id)))
            {
                var description = regulation.GetDescription();

                if (string.IsNullOrEmpty(description))
                    continue;

                if (!isFirstItem) result.Append(", ");

                result.Append(description);
                isFirstItem = false;
            }

            _constraintsDescription.Value = result.Length >= 1 ? result.ToString() : "(None)";
        }

        [Serializable]
        private sealed class ObservableAssetGroupDictionary : ObservableDictionary<string, AssetGroup>
        {
        }

        [Serializable]
        private sealed class
            ObservableAssetLimitationDictionary : SerializeReferenceObservableDictionary<string, IAssetConstraint>
        {
        }
    }
}

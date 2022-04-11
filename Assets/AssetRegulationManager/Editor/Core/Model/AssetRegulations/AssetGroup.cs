﻿// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using AssetRegulationManager.Editor.Core.Shared;
using AssetRegulationManager.Editor.Foundation.OrderCollections;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableProperty;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    /// <summary>
    ///     Rules for defining a group of assets.
    /// </summary>
    [Serializable]
    public sealed class AssetGroup
    {
        [SerializeField] private string _id;
        [SerializeField] private StringObservableProperty _name = new StringObservableProperty("New Asset Group");
        [SerializeField] private AssetFilterObservableDictionary _filters = new AssetFilterObservableDictionary();
        [SerializeField] private StringOrderCollection _filterOrders = new StringOrderCollection();

        private readonly Subject<(string id, int index)> _filterOrderChangedSubject =
            new Subject<(string id, int index)>();

        public IObservable<(string id, int index)> FilterOrderChangedAsObservable => _filterOrderChangedSubject;

        public AssetGroup()
        {
            _id = IdentifierFactory.Create();
        }

        public string Id => _id;

        public IObservableProperty<string> Name => _name;

        /// <summary>
        ///     Filter to determine whether an asset belongs to this group.
        /// </summary>
        public IReadOnlyObservableDictionary<string, IAssetFilter> Filters => _filters;

        public void Setup()
        {
            foreach (var filter in _filters.Values)
                filter?.SetupForMatching();
        }

        /// <summary>
        ///     Return true if the <see cref="assetPath" /> asset belongs to this group.
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public bool Contains(string assetPath)
        {
            if (_filters.Count == 0)
                return false;

            foreach (var filter in _filters.Values)
            {
                if (filter == null)
                    continue;

                if (!filter.IsMatch(assetPath))
                    return false;
            }

            return true;
        }

        public string GetDescription()
        {
            var result = new StringBuilder();
            var isFirstItem = true;
            foreach (var filter in _filters.Values.OrderBy(x => _filterOrders.GetIndex(x.Id)))
            {
                var description = filter.GetDescription();
                
                if (string.IsNullOrEmpty(description))
                    continue;

                if (!isFirstItem)
                    result.Append(" && ");

                result.Append(description);
                isFirstItem = false;
            }

            return result.ToString();
        }

        public T AddFilter<T>() where T : IAssetFilter, new()
        {
            var filter = new T();
            AddFilter(filter);
            return filter;
        }

        public IAssetFilter AddFilter(Type type)
        {
            var filter = (IAssetFilter)Activator.CreateInstance(type);
            AddFilter(filter);
            return filter;
        }

        public void AddFilter<T>(T filter) where T : IAssetFilter
        {
            _filterOrders.Add(filter.Id);
            _filters.Add(filter.Id, filter);
        }

        public void RemoveFilter(string id)
        {
            _filters.Remove(id);
            _filterOrders.Remove(id);
        }

        public int GetFilterOrder(string id)
        {
            return _filterOrders.GetIndex(id);
        }

        public void SetFilterOrder(string id, int index)
        {
            _filterOrders.SetIndex(id, index);
            _filterOrderChangedSubject.OnNext((id, index));
        }
    }
}

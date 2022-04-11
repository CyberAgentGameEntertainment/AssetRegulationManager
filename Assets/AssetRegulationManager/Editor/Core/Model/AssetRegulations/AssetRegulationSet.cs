using System;
using AssetRegulationManager.Editor.Foundation.OrderCollections;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    [Serializable]
    public sealed class AssetRegulationSet
    {
        [SerializeField]
        private AssetRegulationRuleObservableDictionary _values = new AssetRegulationRuleObservableDictionary();

        [SerializeField] private StringOrderCollection _orders = new StringOrderCollection();

        private readonly Subject<(string id, int index)> _orderChangedSubject =
            new Subject<(string id, int index)>();

        public IReadOnlyObservableDictionary<string, AssetRegulation> Values => _values;

        public IObservable<(string id, int index)> OrderChangedAsObservable => _orderChangedSubject;

        public AssetRegulation Add()
        {
            var regulation = new AssetRegulation();
            Add(regulation);
            return regulation;
        }

        public void Add(AssetRegulation regulation)
        {
            _orders.Add(regulation.Id);
            _values.Add(regulation.Id, regulation);
        }

        public void Remove(string id)
        {
            _values.Remove(id);
            _orders.Remove(id);
        }

        public int GetIndex(string id)
        {
            return _orders.GetIndex(id);
        }

        public void SetIndex(string id, int index)
        {
            _orders.SetIndex(id, index);
            _orderChangedSubject.OnNext((id, index));
        }

        public void RefreshGroupDescriptions()
        {
            foreach (var regulation in _values.Values)
                regulation.RefreshTargetsDescription();
        }

        public void RefreshConstraintsDescriptions()
        {
            foreach (var regulation in _values.Values)
                regulation.RefreshConstraintDescription();
        }

        [Serializable]
        private sealed class AssetRegulationRuleObservableDictionary : ObservableDictionary<string, AssetRegulation>
        {
        }
    }
}

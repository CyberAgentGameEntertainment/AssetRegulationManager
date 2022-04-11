using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Foundation.OrderCollections;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    /// <summary>
    ///     View to edit asset groups.
    /// </summary>
    internal sealed class AssetRegulationEditorTargetsPanel : IDisposable
    {
        private const string AddAssetGroupButtonName = "Add Asset Group";
        
        private readonly Subject<Empty> _addAssetGroupButtonClickedSubject = new Subject<Empty>();

        private readonly Dictionary<string, CompositeDisposable> _perItemDisposables =
            new Dictionary<string, CompositeDisposable>();

        public readonly ObservableDictionary<string, AssetGroupView> GroupViews =
            new ObservableDictionary<string, AssetGroupView>();

        private readonly StringOrderCollection _groupViewOrders = new StringOrderCollection();

        public bool Enabled { get; set; }

        /// <summary>
        ///     IObservable to observe add button pressed.
        /// </summary>
        public IObservable<Empty> AddAssetGroupButtonClickedAsObservable => _addAssetGroupButtonClickedSubject;

        public void Dispose()
        {
            foreach (var disposable in _perItemDisposables.Values)
                disposable.Dispose();

            _addAssetGroupButtonClickedSubject.Dispose();
        }

        /// <summary>
        ///     Add a view to draw asset group.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AssetGroupView AddAssetGroup(string id)
        {
            var disposables = new CompositeDisposable();
            var view = new AssetGroupView(id);
            _groupViewOrders.Add(id);
            GroupViews.Add(id, view);

            _perItemDisposables.Add(id, disposables);

            return view;
        }

        /// <summary>
        ///     Remove a view to draw asset group.
        /// </summary>
        /// <param name="id"></param>
        public void RemoveAssetGroup(string id)
        {
            var disposable = _perItemDisposables[id];
            disposable.Dispose();
            _perItemDisposables.Remove(id);
            var view = GroupViews[id];
            view.Dispose();
            GroupViews.Remove(id);
            _groupViewOrders.Remove(id);
        }

        /// <summary>
        ///     Clear all views to draw asset group.
        /// </summary>
        public void ClearAssetGroups()
        {
            foreach (var disposable in _perItemDisposables.Values)
                disposable.Dispose();
            _perItemDisposables.Clear();

            foreach (var groupRuleView in GroupViews.Values)
                groupRuleView.Dispose();
            GroupViews.Clear();
            _groupViewOrders.Clear();
        }

        public void SetAssetGroupOrder(string id, int index)
        {
            _groupViewOrders.SetIndex(id, index);
        }

        /// <summary>
        ///     Draw GUI using GUILayout.
        /// </summary>
        public void DoLayout()
        {
            if (!Enabled)
                return;

            foreach (var view in GroupViews.Values.OrderBy(x => _groupViewOrders.GetIndex(x.AssetGroupId)))
                view.DoLayout();

            GUILayout.Space(4);

            // Add Asset Group Button
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(AddAssetGroupButtonName, GUILayout.MinWidth(120)))
                    _addAssetGroupButtonClickedSubject.OnNext(Empty.Default);

                GUILayout.FlexibleSpace();
            }
        }
    }
}

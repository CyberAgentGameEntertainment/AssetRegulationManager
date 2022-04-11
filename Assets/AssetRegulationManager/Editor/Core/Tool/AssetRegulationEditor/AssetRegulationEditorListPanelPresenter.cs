using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorListPanelPresenter : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private readonly Dictionary<string, CompositeDisposable> _perItemDisposables =
            new Dictionary<string, CompositeDisposable>();

        private readonly AssetRegulationEditorListPanel _view;

        public AssetRegulationEditorListPanelPresenter(AssetRegulationSet set,
            AssetRegulationEditorListPanel view)
        {
            Assert.IsNotNull(set);
            Assert.IsNotNull(view);

            _view = view;

            set.Values
                .ObservableAdd
                .Subscribe(OnRegulationAdded)
                .DisposeWith(_disposables);

            set.Values
                .ObservableRemove
                .Subscribe(OnRegulationRemoved)
                .DisposeWith(_disposables);

            set.Values
                .ObservableClear
                .Subscribe(OnRegulationsCleared)
                .DisposeWith(_disposables);

            set.OrderChangedAsObservable
                .Subscribe(x =>
                {
                    var item = _view.TreeView.GetItemByRegulationId(x.id);
                    _view.TreeView.SetItemIndex(item.id, x.index, false);
                }).DisposeWith(_disposables);

            foreach (var regulation in set.Values.Values.OrderBy(x => set.GetIndex(x.Id)))
                AddTreeViewItem(regulation, false);

            view.TreeView.Reload();
        }

        public void Dispose()
        {
            foreach (var disposable in _perItemDisposables.Values) disposable.Dispose();
            _disposables.Dispose();
            ClearTreeViewItems(false);
        }

        private void OnRegulationAdded(DictionaryAddEvent<string, AssetRegulation> addEvent)
        {
            var regulation = addEvent.Value;
            AddTreeViewItem(regulation, true);
        }

        private void OnRegulationRemoved(DictionaryRemoveEvent<string, AssetRegulation> removeEvent)
        {
            var regulationId = removeEvent.Key;
            RemoveTreeViewItem(regulationId, true);
        }

        private void OnRegulationsCleared(Empty empty)
        {
            ClearTreeViewItems(true);
        }

        private AssetRegulationEditorTreeViewItem AddTreeViewItem(AssetRegulation regulation, bool reload)
        {
            var disposables = new CompositeDisposable();
            var item = _view.TreeView.AddItem(regulation.Id);
            regulation.Name
                .Subscribe(x => item.SetName(x))
                .DisposeWith(disposables);

            regulation.AssetGroupsDescription
                .Subscribe(x => item.TargetsDescription = x)
                .DisposeWith(disposables);

            regulation.ConstraintsDescription
                .Subscribe(x => item.ConstraintsDescription = x)
                .DisposeWith(disposables);

            _perItemDisposables.Add(regulation.Id, disposables);

            if (reload) _view.TreeView.Reload();

            return item;
        }

        private void RemoveTreeViewItem(string regulationId, bool reload)
        {
            var disposable = _perItemDisposables[regulationId];
            disposable.Dispose();
            _perItemDisposables.Remove(regulationId);
            _view.TreeView.RemoveItem(regulationId);

            if (reload) _view.TreeView.Reload();
        }

        private void ClearTreeViewItems(bool reload)
        {
            foreach (var disposable in _perItemDisposables)
                disposable.Value.Dispose();

            _perItemDisposables.Clear();
            _view.TreeView.ClearItems();

            if (reload) _view.TreeView.Reload();
        }
    }
}

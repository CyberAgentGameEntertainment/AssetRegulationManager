using System;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using AssetRegulationManager.Editor.Foundation.TinyRx.ObservableCollection;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    /// <summary>
    ///     Draw the <see cref="AssetRegulationEditorTargetsPanel" />.
    /// </summary>
    internal sealed class AssetRegulationEditorTargetsPanelPresenter : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly AssetRegulation _regulation;
        private readonly AssetRegulationEditorTargetsPanel _view;

        public AssetRegulationEditorTargetsPanelPresenter(AssetRegulation regulation,
            AssetRegulationEditorTargetsPanel view)
        {
            Assert.IsNotNull(regulation);
            Assert.IsNotNull(view);

            _regulation = regulation;
            _view = view;
            view.Enabled = true;

            regulation.AssetGroups.ObservableAdd
                .Subscribe(x => AddListItem(x.Value))
                .DisposeWith(_disposables);

            regulation.AssetGroups.ObservableRemove
                .Subscribe(OnItemRemoved)
                .DisposeWith(_disposables);

            regulation.AssetGroups.ObservableClear
                .Subscribe(OnItemsCleared)
                .DisposeWith(_disposables);

            regulation.AssetGroupOrderChangedAsObservable
                .Subscribe(x => SetAssetGroupOrder(x.id, x.index))
                .DisposeWith(_disposables);

            foreach (var groupRule in regulation.AssetGroups.Values.OrderBy(x => regulation.GetAssetGroupOrder(x.Id)))
                AddListItem(groupRule);
        }

        public void Dispose()
        {
            _view.ClearAssetGroups();
            _disposables.Dispose();
            _view.Enabled = false;
        }

        private void AddListItem(AssetGroup group)
        {
            _view.AddAssetGroup(group.Id);
            var order = _regulation.GetAssetGroupOrder(group.Id);
            _view.SetAssetGroupOrder(group.Id, order);
        }

        private void OnItemRemoved(DictionaryRemoveEvent<string, AssetGroup> removeEvent)
        {
            RemoveListItem(removeEvent.Value);
        }

        private void RemoveListItem(AssetGroup group)
        {
            _view.RemoveAssetGroup(group.Id);
        }

        private void OnItemsCleared(Empty _)
        {
            ClearListItems();
        }

        private void ClearListItems()
        {
            _view.ClearAssetGroups();
        }

        private void SetAssetGroupOrder(string id, int order)
        {
            _view.SetAssetGroupOrder(id, order);
        }
    }
}

using System;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Shared;
using AssetRegulationManager.Editor.Foundation.TinyRx;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    /// <summary>
    ///     Draw the <see cref="AssetGroupView" />.
    /// </summary>
    internal sealed class AssetGroupViewPresenter : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public AssetGroupViewPresenter(AssetGroup assetGroup, AssetGroupView view)
        {
            assetGroup.Name
                .Subscribe(x => view.Name.Value = x)
                .DisposeWith(_disposables);

            assetGroup.Filters.ObservableAdd
                .Subscribe(x =>
                {
                    var filter = x.Value;
                    view.AddFilter(filter);
                    view.SetFilterOrder(filter.Id, assetGroup.GetFilterOrder(filter.Id));
                })
                .DisposeWith(_disposables);

            assetGroup.Filters.ObservableRemove
                .Subscribe(x => view.RemoveFilter(x.Value.Id))
                .DisposeWith(_disposables);

            assetGroup.Filters.ObservableClear
                .Subscribe(_ => view.ClearFilters())
                .DisposeWith(_disposables);

            assetGroup.FilterOrderChangedAsObservable
                .Subscribe(x => view.SetFilterOrder(x.id, x.index))
                .DisposeWith(_disposables);

            foreach (var filter in assetGroup.Filters.Values.OrderBy(x => assetGroup.GetFilterOrder(x.Id)))
            {
                view.AddFilter(filter);
                view.SetFilterOrder(filter.Id, assetGroup.GetFilterOrder(filter.Id));
            }

            view.SetupClipboard(() => ObjectCopyBuffer.Type == typeof(AssetGroup),
                () => ObjectCopyBuffer.Type == typeof(AssetGroup),
                () => typeof(IAssetFilter).IsAssignableFrom(ObjectCopyBuffer.Type), x =>
                {
                    if (!assetGroup.Filters.TryGetValue(x, out var filter))
                        return false;

                    return filter.GetType() == ObjectCopyBuffer.Type;
                });
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}

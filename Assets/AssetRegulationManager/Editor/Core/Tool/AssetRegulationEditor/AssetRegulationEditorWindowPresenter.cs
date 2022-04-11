using System;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Shared;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    /// <summary>
    ///     Draw the <see cref="AssetRegulationEditorWindow" />.
    /// </summary>
    internal sealed class AssetRegulationEditorWindowPresenter : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly ObserveAssetDeletionService _observeAssetDeletionService;
        private readonly AssetRegulationEditorWindow _view;

        public AssetRegulationEditorWindowPresenter(AssetRegulationSetStore store,
            AssetRegulationEditorWindow view)
        {
            Assert.IsNotNull(store);
            Assert.IsNotNull(view);

            _view = view;

            // If AssetRegulationRuleSetStore asset is deleted, show the empty view.
            _observeAssetDeletionService = new ObserveAssetDeletionService(store);
            _observeAssetDeletionService.AssetDeletedAsObservable
                .Subscribe(_ => SetupEmptyView())
                .DisposeWith(_disposables);

            // Setup the panel to edit the store.
            SetupEditorView();
        }

        public void Dispose()
        {
            _observeAssetDeletionService.Dispose();
            _disposables.Dispose();
        }

        private void SetupEditorView()
        {
            _view.ActiveViewType = AssetRegulationEditorWindow.ViewType.Editor;
        }

        private void SetupEmptyView()
        {
            _view.ActiveViewType = AssetRegulationEditorWindow.ViewType.Empty;
        }
    }
}

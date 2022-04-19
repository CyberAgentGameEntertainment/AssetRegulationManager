using System;
using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Shared;
using AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.ApplicationService;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    /// <summary>
    ///     Process events from the <see cref="AssetRegulationEditorTargetsPanel" />.
    /// </summary>
    internal sealed class AssetRegulationEditorTargetsPanelController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private readonly EditAssetRegulationService _editService;
        private readonly EditObjectService _editObjectService;

        private readonly Dictionary<string, AssetGroupViewController> _groupViewControllers =
            new Dictionary<string, AssetGroupViewController>();

        private readonly Dictionary<string, AssetGroupViewPresenter> _groupViewPresenters =
            new Dictionary<string, AssetGroupViewPresenter>();

        private readonly Dictionary<string, CompositeDisposable> _perItemDisposables =
            new Dictionary<string, CompositeDisposable>();

        private readonly AssetRegulation _regulation;

        public AssetRegulationEditorTargetsPanelController(AssetRegulation regulation,
            AssetRegulationEditorTargetsPanel view, EditAssetRegulationService editService,
            EditObjectService editObjectService)
        {
            Assert.IsNotNull(regulation);
            Assert.IsNotNull(view);
            Assert.IsNotNull(editService);
            Assert.IsNotNull(editObjectService);

            _regulation = regulation;
            _editService = editService;
            _editObjectService = editObjectService;

            view.AddAssetGroupButtonClickedAsObservable
                .Subscribe(_ => editService.AddAssetGroup())
                .DisposeWith(_disposables);

            view.PasteAssetGroupMenuExecutedAsObservable
                .Subscribe(_ => editService.PasteAssetGroup())
                .DisposeWith(_disposables);

            view.GroupViews.ObservableAdd
                .Subscribe(x => SetupAssetGroupView(x.Value))
                .DisposeWith(_disposables);

            view.GroupViews.ObservableRemove
                .Subscribe(x => CleanupAssetGroupView(x.Key))
                .DisposeWith(_disposables);

            view.GroupViews.ObservableClear
                .Subscribe(_ => CleanupAllAssetGroupViews())
                .DisposeWith(_disposables);

            foreach (var x in view.GroupViews)
                SetupAssetGroupView(x.Value);
        }

        public void Dispose()
        {
            foreach (var disposable in _perItemDisposables.Values)
                disposable.Dispose();
            _perItemDisposables.Clear();

            foreach (var controller in _groupViewControllers.Values)
                controller.Dispose();
            _groupViewControllers.Clear();

            foreach (var presenter in _groupViewPresenters.Values)
                presenter.Dispose();
            _groupViewPresenters.Clear();

            _disposables.Dispose();
        }

        private void SetupAssetGroupView(AssetGroupView view)
        {
            var disposables = new CompositeDisposable();
            var assetGroupRuleId = view.AssetGroupId;
            _perItemDisposables.Add(assetGroupRuleId, disposables);
            var assetGroupRule = _regulation.AssetGroups[assetGroupRuleId];

            var editAssetGroupRuleService = new EditAssetGroupService(assetGroupRule, _editObjectService);

            var controller = new AssetGroupViewController(view, _editService, editAssetGroupRuleService);
            _groupViewControllers.Add(assetGroupRuleId, controller);

            var presenter = new AssetGroupViewPresenter(assetGroupRule, view);
            _groupViewPresenters.Add(assetGroupRuleId, presenter);
        }

        private void CleanupAssetGroupView(string assetGroupRuleId)
        {
            var disposables = _perItemDisposables[assetGroupRuleId];
            disposables.Dispose();
            _perItemDisposables.Remove(assetGroupRuleId);

            var presenter = _groupViewPresenters[assetGroupRuleId];
            presenter.Dispose();
            _groupViewPresenters.Remove(assetGroupRuleId);

            var controller = _groupViewControllers[assetGroupRuleId];
            controller.Dispose();
            _groupViewControllers.Remove(assetGroupRuleId);
        }

        private void CleanupAllAssetGroupViews()
        {
            foreach (var disposable in _perItemDisposables.Values)
                disposable.Dispose();
            _perItemDisposables.Clear();

            foreach (var presenter in _groupViewPresenters.Values)
                presenter.Dispose();
            _groupViewPresenters.Clear();

            foreach (var controller in _groupViewControllers.Values)
                controller.Dispose();
            _groupViewControllers.Clear();
        }
    }
}

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

        private readonly Dictionary<string, AssetGroupRuleViewController> _groupRuleViewControllers =
            new Dictionary<string, AssetGroupRuleViewController>();

        private readonly Dictionary<string, AssetGroupRuleViewPresenter> _groupRuleViewPresenters =
            new Dictionary<string, AssetGroupRuleViewPresenter>();

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

            _groupRuleViewControllers.Clear();

            foreach (var controller in _groupRuleViewControllers.Values)
                controller.Dispose();
            _groupRuleViewControllers.Clear();

            foreach (var presenter in _groupRuleViewPresenters.Values)
                presenter.Dispose();
            _groupRuleViewPresenters.Clear();

            _disposables.Dispose();
        }

        private void SetupAssetGroupView(AssetGroupView view)
        {
            var disposables = new CompositeDisposable();
            var assetGroupRuleId = view.AssetGroupId;
            _perItemDisposables.Add(assetGroupRuleId, disposables);
            var assetGroupRule = _regulation.AssetGroups[assetGroupRuleId];

            var editAssetGroupRuleService = new EditAssetGroupService(assetGroupRule, _editObjectService);

            var controller = new AssetGroupRuleViewController(view, _editService, editAssetGroupRuleService);
            _groupRuleViewControllers.Add(assetGroupRuleId, controller);

            var presenter = new AssetGroupRuleViewPresenter(assetGroupRule, view);
            _groupRuleViewPresenters.Add(assetGroupRuleId, presenter);
        }

        private void CleanupAssetGroupView(string assetGroupRuleId)
        {
            var disposables = _perItemDisposables[assetGroupRuleId];
            disposables.Dispose();
            _perItemDisposables.Remove(assetGroupRuleId);

            var presenter = _groupRuleViewPresenters[assetGroupRuleId];
            presenter.Dispose();
            _groupRuleViewPresenters.Remove(assetGroupRuleId);

            var controller = _groupRuleViewControllers[assetGroupRuleId];
            controller.Dispose();
            _groupRuleViewControllers.Remove(assetGroupRuleId);
        }

        private void CleanupAllAssetGroupViews()
        {
            foreach (var disposable in _perItemDisposables.Values)
                disposable.Dispose();
            _perItemDisposables.Clear();

            foreach (var presenter in _groupRuleViewPresenters.Values)
                presenter.Dispose();
            _groupRuleViewPresenters.Clear();

            foreach (var controller in _groupRuleViewControllers.Values)
                controller.Dispose();
            _groupRuleViewControllers.Clear();
        }
    }
}

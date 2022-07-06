using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Shared;
using AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.ApplicationService;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEditor.IMGUI.Controls;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    /// <summary>
    ///     Process events from the <see cref="AssetRegulationEditorWindow" />.
    /// </summary>
    internal sealed class AssetRegulationEditorWindowController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly EditObjectService _editObjectService;
        private readonly EditAssetRegulationSetService _editSetService;
        private readonly ObserveAssetDeletionService _observeAssetDeletionService;
        private readonly AssetRegulationSetStore _store;
        private readonly AssetRegulationEditorWindow _view;

        private AssetRegulationEditorListPanelController _listPanelController;
        private AssetRegulationEditorListPanelPresenter _listPanelPresenter;
        private AssetRegulationEditorTargetsPanelController _targetsPanelController;
        private AssetRegulationEditorTargetsPanelPresenter _targetsPanelPresenter;
        private AssetRegulationEditorConstraintsPanelController _constraintsPanelController;
        private AssetRegulationEditorConstraintsPanelPresenter _constraintsPanelPresenter;

        public AssetRegulationEditorWindowController(AssetRegulationSetStore store,
            AssetRegulationEditorWindow view, EditAssetRegulationSetService editSetService,
            EditObjectService editObjectService)
        {
            Assert.IsNotNull(store);
            Assert.IsNotNull(view);
            Assert.IsNotNull(editSetService);
            Assert.IsNotNull(editObjectService);

            _store = store;
            _view = view;
            _editSetService = editSetService;
            _editObjectService = editObjectService;
            _observeAssetDeletionService = new ObserveAssetDeletionService(store);

            // If AssetRegulationRuleSetStore asset is deleted, show the empty view.
            _observeAssetDeletionService.AssetDeletedAsObservable
                .Subscribe(_ => SetupEmptyView())
                .DisposeWith(_disposables);

            // Observe shortcut commands.
            view.UndoShortcutExecutedAsObservable
                .Subscribe(_ => _editObjectService.Undo())
                .DisposeWith(_disposables);

            view.RedoShortcutExecutedAsObservable
                .Subscribe(_ => _editObjectService.Redo())
                .DisposeWith(_disposables);

            view.RemoveShortcutExecutedAsObservable
                .Subscribe(items =>
                {
                    var regulationIds = items
                        .Select(x => x.RegulationId);
                    _editSetService.RemoveRegulations(regulationIds);
                }).DisposeWith(_disposables);

            // Setup the panel to edit the store.
            SetupEditorView();
        }

        public void Dispose()
        {
            if (_listPanelController != null)
            {
                _listPanelController.Dispose();
                _listPanelPresenter.Dispose();

                _view.ListPanel.TreeView.OnSelectionChanged -= OnTreeViewSelectionChanged;
                _view.ListPanel.TreeView.OnItemRemoved -= OnTreeViewItemRemoved;
            }

            _targetsPanelPresenter?.Dispose();
            _targetsPanelController?.Dispose();
            _constraintsPanelPresenter?.Dispose();
            _constraintsPanelController?.Dispose();
            _observeAssetDeletionService.Dispose();
            _disposables.Dispose();
        }

        private void SetupEditorView()
        {
            CleanupEditorView();

            // Setup the list panel.
            _listPanelController =
                new AssetRegulationEditorListPanelController(_store.Set, _editSetService, _view.ListPanel);
            _listPanelPresenter = new AssetRegulationEditorListPanelPresenter(_store.name, _store.Set, _view.ListPanel);

            _view.ListPanel.TreeView.OnSelectionChanged += OnTreeViewSelectionChanged;
            _view.ListPanel.TreeView.OnItemRemoved += OnTreeViewItemRemoved;

            // Setup the targets/constraints panel.
            var treeViewItemIds = _view.ListPanel.TreeView.GetSelection();
            if (treeViewItemIds == null || treeViewItemIds.Count == 0)
            {
                SetupTargetsPanel(null);
                SetupConstraintsPanel(null);
            }
            else
            {
                var itemId = treeViewItemIds[0];
                SetupTargetsPanel(itemId);
                SetupConstraintsPanel(itemId);
            }
        }

        private void CleanupEditorView()
        {
            if (_listPanelController != null)
            {
                _listPanelPresenter.Dispose();
                _listPanelController.Dispose();
                _view.ListPanel.TreeView.OnSelectionChanged -= OnTreeViewSelectionChanged;
                _view.ListPanel.TreeView.OnItemRemoved -= OnTreeViewItemRemoved;
            }

            _targetsPanelPresenter?.Dispose();
            _targetsPanelController?.Dispose();
            _constraintsPanelPresenter?.Dispose();
            _constraintsPanelController?.Dispose();
            _listPanelController = null;
            _listPanelPresenter = null;
            _targetsPanelPresenter = null;
            _targetsPanelController = null;
        }

        private void SetupEmptyView()
        {
            CleanupEditorView();
        }

        private void OnTreeViewSelectionChanged(IList<int> _)
        {
            SetupTargetsAndConstraintsPanels();
        }

        private void OnTreeViewItemRemoved(TreeViewItem _)
        {
            SetupTargetsAndConstraintsPanels();
        }

        private void SetupTargetsAndConstraintsPanels()
        {
            var treeViewItemIds = _view.ListPanel.TreeView.GetSelection();
            if (treeViewItemIds == null || treeViewItemIds.Count == 0)
            {
                SetupTargetsPanel(null);
                SetupConstraintsPanel(null);
                return;
            }

            var itemId = treeViewItemIds[0];
            SetupTargetsPanel(itemId);
            SetupConstraintsPanel(itemId);
        }

        private void SetupTargetsPanel(int treeViewItemId)
        {
            var treeView = _view.ListPanel.TreeView;
            if (!treeView.HasItem(treeViewItemId))
            {
                SetupTargetsPanel(null);
                return;
            }

            var item = (AssetRegulationEditorTreeViewItem)treeView.GetItem(treeViewItemId);
            SetupTargetsPanel(item);
        }

        private void SetupTargetsPanel(AssetRegulationEditorTreeViewItem item)
        {
            _targetsPanelPresenter?.Dispose();
            _targetsPanelController?.Dispose();

            // Reset view state.
            _view.TargetsPanel.ClearAssetGroups();

            // If item does not exits, do nothing.
            if (item == null)
                return;

            // Create the controller and the presenter.
            var regulation = _store.Set.Values[item.RegulationId];
            var editRegulationService = new EditAssetRegulationService(regulation, _editObjectService);
            _targetsPanelController =
                new AssetRegulationEditorTargetsPanelController(regulation, _view.TargetsPanel, editRegulationService,
                    _editObjectService);
            _targetsPanelPresenter = new AssetRegulationEditorTargetsPanelPresenter(regulation, _view.TargetsPanel);
        }

        private void SetupConstraintsPanel(int treeViewItemId)
        {
            var treeView = _view.ListPanel.TreeView;
            if (!treeView.HasItem(treeViewItemId))
            {
                SetupConstraintsPanel(null);
                return;
            }

            var item = (AssetRegulationEditorTreeViewItem)treeView.GetItem(treeViewItemId);
            SetupConstraintsPanel(item);
        }

        private void SetupConstraintsPanel(AssetRegulationEditorTreeViewItem item)
        {
            _constraintsPanelController?.Dispose();
            _constraintsPanelPresenter?.Dispose();

            // Reset view state.
            _view.ConstraintsPanel.ClearConstraints();

            // If item does not exits, do nothing.
            if (item == null)
                return;

            // Create the controller and the presenter.
            var regulation = _store.Set.Values[item.RegulationId];
            var editRegulationService = new EditAssetRegulationService(regulation, _editObjectService);
            _constraintsPanelController =
                new AssetRegulationEditorConstraintsPanelController(_view.ConstraintsPanel, editRegulationService);
            _constraintsPanelPresenter =
                new AssetRegulationEditorConstraintsPanelPresenter(regulation, _view.ConstraintsPanel);
        }
    }
}

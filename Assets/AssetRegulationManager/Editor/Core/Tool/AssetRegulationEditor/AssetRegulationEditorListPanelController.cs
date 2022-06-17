using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.ApplicationService;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorListPanelController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly EditAssetRegulationSetService _editService;

        private readonly Dictionary<int, CompositeDisposable> _perItemDisposables =
            new Dictionary<int, CompositeDisposable>();

        private readonly AssetRegulationSet _regulations;
        private readonly AssetRegulationEditorListPanel _view;

        public AssetRegulationEditorListPanelController(AssetRegulationSet regulations,
            EditAssetRegulationSetService editService, AssetRegulationEditorListPanel view)
        {
            Assert.IsNotNull(editService);
            Assert.IsNotNull(view);

            _regulations = regulations;
            _editService = editService;
            _view = view;

            view.AddButtonClickedAsObservable
                .Subscribe(_ => _editService.AddRegulation())
                .DisposeWith(_disposables);

            view.TreeView
                .ItemIndexChangedAsObservable
                .Subscribe(x => _editService.ChangeRuleIndex(x.item.RegulationId, x.newIndex))
                .DisposeWith(_disposables);

            view.TreeView
                .OnItemAddedAsObservable()
                .Subscribe(x =>
                {
                    var disposables = new CompositeDisposable();
                    disposables.DisposeWith(_disposables);
                    x.Name
                        .Skip(1)
                        .Subscribe(y => _editService.ChangeRuleName(x.RegulationId, y))
                        .DisposeWith(disposables);
                    _perItemDisposables.Add(x.id, disposables);
                }).DisposeWith(_disposables);

            view.RightClickCreateMenuClickedAsObservable
                .Subscribe(_ => _editService.AddRegulation())
                .DisposeWith(_disposables);

            view.RightClickRemoveMenuClickedAsObservable
                .Subscribe(items =>
                {
                    var regulationIds = items.Select(x => x.RegulationId);
                    _editService.RemoveRegulations(regulationIds);
                }).DisposeWith(_disposables);

            view.RightClickCopyTargetsDescriptionMenuClickedSubject
                .Subscribe(_ => CopyTargetsDescription())
                .DisposeWith(_disposables);

            view.RightClickCopyConstraintsDescriptionMenuClickedSubject
                .Subscribe(_ => CopyConstraintsDescription())
                .DisposeWith(_disposables);

            view.AssetSelectButtonClickedAsObservable
                .Subscribe(_ => ShowAssetSelectMenu())
                .DisposeWith(_disposables);
        }

        public void Dispose()
        {
            foreach (var disposable in _perItemDisposables.Values)
                disposable.Dispose();
            _perItemDisposables.Clear();
            _disposables.Dispose();
        }

        private void CopyTargetsDescription()
        {
            var selection = _view.TreeView.GetSelection();
            if (selection == null || selection.Count == 0) return;

            var itemId = selection[0];
            if (!_view.TreeView.HasItem(itemId)) return;

            var item = (AssetRegulationEditorTreeViewItem)_view.TreeView.GetItem(itemId);
            CopyTargetsDescription(item.RegulationId);
        }

        private void CopyTargetsDescription(string regulationId)
        {
            var regulation = _regulations.Values[regulationId];
            var description = regulation.AssetGroupsDescription.Value;
            GUIUtility.systemCopyBuffer = description;
        }

        private void CopyConstraintsDescription()
        {
            var selection = _view.TreeView.GetSelection();
            if (selection == null || selection.Count == 0) return;

            var itemId = selection[0];
            if (!_view.TreeView.HasItem(itemId)) return;

            var item = (AssetRegulationEditorTreeViewItem)_view.TreeView.GetItem(itemId);
            CopyConstraintsDescription(item.RegulationId);
        }

        private void CopyConstraintsDescription(string regulationId)
        {
            var regulation = _regulations.Values[regulationId];
            var description = regulation.ConstraintsDescription.Value;
            GUIUtility.systemCopyBuffer = description;
        }

        private void ShowAssetSelectMenu()
        {
            var settingsAssets = AssetDatabase
                .FindAssets($"t:{nameof(AssetRegulationSetStore)}")
                .Select(x =>
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(x);
                    return AssetDatabase.LoadAssetAtPath<AssetRegulationSetStore>(assetPath);
                }).ToArray();

            var menu = new GenericMenu();
            foreach (var settingsAsset in settingsAssets.OrderBy(x => x.name))
                menu.AddItem(new GUIContent(settingsAsset.name), false,
                    () => AssetRegulationEditorWindow.Open(settingsAsset));

            menu.AddSeparator(string.Empty);
            menu.AddItem(new GUIContent("Create New"), false, () =>
            {
                var assetPath = EditorUtility.SaveFilePanelInProject("Save", "AssetRegulationData", "asset", "");
                if (string.IsNullOrEmpty(assetPath))
                    return;

                var asset = ScriptableObject.CreateInstance<AssetRegulationSetStore>();
                AssetDatabase.CreateAsset(asset, assetPath);
                AssetRegulationEditorWindow.Open(asset);
            });
            menu.ShowAsContext();
        }
    }
}

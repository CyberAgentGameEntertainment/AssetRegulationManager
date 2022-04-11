using System;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Shared;
using AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.ApplicationService;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    internal sealed class AssetRegulationEditorApplication : IDisposable
    {
        private readonly AssetRegulationEditorWindowController _controller;
        private readonly EditObjectService _editObjectService;
        private readonly AssetRegulationEditorWindowPresenter _presenter;
        private readonly AssetRegulationSetStore _store;

        public AssetRegulationEditorApplication(AssetRegulationSetStore store, AssetRegulationEditorWindow window)
        {
            _store = store;
            _editObjectService = new EditObjectService(store);
            var editRulesService = new EditAssetRegulationSetService(store.Set, _editObjectService);
            _controller =
                new AssetRegulationEditorWindowController(store, window, editRulesService, _editObjectService);
            _presenter = new AssetRegulationEditorWindowPresenter(store, window);

            EditorApplication.update += Update;
        }

        public void Dispose()
        {
            EditorApplication.update -= Update;

            _editObjectService.Dispose();
            _controller.Dispose();
            _presenter.Dispose();
        }

        private void Update()
        {
            _store.Set.RefreshGroupDescriptions();
            _store.Set.RefreshConstraintsDescriptions();
        }
    }
}

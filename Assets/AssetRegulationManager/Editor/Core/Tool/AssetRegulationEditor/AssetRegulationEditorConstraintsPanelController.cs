using System;
using System.Linq;
using System.Reflection;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.ApplicationService;
using AssetRegulationManager.Editor.Foundation.TinyRx;
using UnityEditor;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor
{
    /// <summary>
    ///     Process events from the <see cref="AssetRegulationEditorConstraintsPanel" />.
    /// </summary>
    internal sealed class AssetRegulationEditorConstraintsPanelController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly EditAssetRegulationService _editRegulationService;

        public AssetRegulationEditorConstraintsPanelController(AssetRegulationEditorConstraintsPanel view,
            EditAssetRegulationService editRegulationService)
        {
            _editRegulationService = editRegulationService;

            view.AddConstraintButtonClickedAsObservable
                .Subscribe(_ => OnAddConstraintsButtonClicked())
                .DisposeWith(_disposables);

            view.RemoveConstraintMenuExecutedAsObservable
                .Subscribe(editRegulationService.RemoveConstraint)
                .DisposeWith(_disposables);

            view.ConstraintValueChangedAsObservable
                .Subscribe(editRegulationService.OnConstraintValueChanged)
                .DisposeWith(_disposables);

            view.MoveUpMenuExecutedAsObservable
                .Subscribe(editRegulationService.MoveUpConstraintOrder)
                .DisposeWith(_disposables);

            view.MoveDownMenuExecutedObservable
                .Subscribe(editRegulationService.MoveDownConstraintOrder)
                .DisposeWith(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void OnAddConstraintsButtonClicked()
        {
            var types = TypeCache.GetTypesDerivedFrom<IAssetConstraint>()
                .Where(x => !x.IsAbstract && x.GetCustomAttribute<IgnoreAssetConstraintAttribute>() == null);

            var menu = new GenericMenu();
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<AssetConstraintAttribute>();
                var menuName = attribute == null ? ObjectNames.NicifyVariableName(type.Name) : attribute.MenuName;
                menu.AddItem(new GUIContent(menuName), false, () => _editRegulationService.AddConstraint(type));
            }

            menu.ShowAsContext();
        }
    }
}

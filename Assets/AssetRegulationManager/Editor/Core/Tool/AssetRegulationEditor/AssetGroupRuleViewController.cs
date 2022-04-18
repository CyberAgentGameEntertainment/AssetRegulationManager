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
    ///     Process events from the <see cref="AssetGroupView" />.
    /// </summary>
    internal sealed class AssetGroupRuleViewController : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly EditAssetGroupService _editGroupService;

        public AssetGroupRuleViewController(AssetGroupView view,
            EditAssetRegulationService editRegulationService, EditAssetGroupService editGroupService)
        {
            _editGroupService = editGroupService;

            view.Name.Skip(1)
                .Subscribe(editGroupService.SetName)
                .DisposeWith(_disposables);

            view.RemoveGroupMenuExecutedAsObservable
                .Subscribe(x => editRegulationService.RemoveAssetGroup(view.AssetGroupId))
                .DisposeWith(_disposables);

            view.MoveUpMenuExecutedAsObservable
                .Subscribe(_ => editRegulationService.MoveUpAssetGroupOrder(view.AssetGroupId))
                .DisposeWith(_disposables);

            view.MoveDownMenuExecutedObservable
                .Subscribe(_ => editRegulationService.MoveDownAssetGroupOrder(view.AssetGroupId))
                .DisposeWith(_disposables);

            view.AddFilterButtonClickedAsObservable
                .Subscribe(_ => OnAddFilterButtonClicked())
                .DisposeWith(_disposables);

            view.RemoveFilterMenuExecutedAsObservable
                .Subscribe(editGroupService.RemoveFilter)
                .DisposeWith(_disposables);

            view.MoveUpFilterMenuExecutedAsObservable
                .Subscribe(editGroupService.MoveUpFilterOrder)
                .DisposeWith(_disposables);

            view.MoveDownFilterMenuExecutedAsObservable
                .Subscribe(editGroupService.MoveDownAssetGroupOrder)
                .DisposeWith(_disposables);

            view.MouseDownAsObservable
                .Subscribe(_ => editGroupService.OnMouseButtonClicked())
                .DisposeWith(_disposables);

            view.FilterValueChangedAsObservable
                .Subscribe(editGroupService.RegisterFilterHistory)
                .DisposeWith(_disposables);

            view.Filters.ObservableAdd
                .Subscribe(x => editGroupService.SetupFilterHistory(x.Key))
                .DisposeWith(_disposables);

            view.Filters.ObservableRemove
                .Subscribe(x => editGroupService.CleanupFilterHistory(x.Key))
                .DisposeWith(_disposables);

            view.Filters.ObservableClear
                .Subscribe(x => editGroupService.CleanupFilterHistories())
                .DisposeWith(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void OnAddFilterButtonClicked()
        {
            // Get types of all asset filter.
            var types = TypeCache.GetTypesDerivedFrom<IAssetFilter>()
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetCustomAttribute<IgnoreAssetFilterAttribute>() == null);

            // Show filter selection menu.
            var menu = new GenericMenu();
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<AssetFilterAttribute>();
                var menuName = attribute == null ? ObjectNames.NicifyVariableName(type.Name) : attribute.MenuName;
                menu.AddItem(new GUIContent(menuName), false, () => _editGroupService.AddFilter(type));
            }

            menu.ShowAsContext();
        }
    }
}

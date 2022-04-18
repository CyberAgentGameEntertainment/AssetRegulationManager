using System;
using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Shared;
using AssetRegulationManager.Editor.Foundation.StateBasedUndo;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.ApplicationService
{
    /// <summary>
    ///     Application Service to edit <see cref="AssetGroup" />.
    /// </summary>
    internal class EditAssetGroupService
    {
        private readonly AssetGroup _assetGroup;
        private readonly EditObjectService _editObjectService;

        private readonly Dictionary<string, History> _perFilterStateBasedHistory = new Dictionary<string, History>();
        private int _commandId;
        private int _mouseButtonClickedCount;

        public EditAssetGroupService(AssetGroup assetGroup, EditObjectService editObjectService)
        {
            _assetGroup = assetGroup;
            _editObjectService = editObjectService;
        }

        public void SetName(string name)
        {
            var oldName = _assetGroup.Name.Value;

            if (name == oldName)
                return;

            _editObjectService.Edit($"Edit Asset Group Name {_assetGroup.Id}",
                () => _assetGroup.Name.Value = name,
                () => _assetGroup.Name.Value = oldName);
        }

        public void AddFilter<T>() where T : IAssetFilter, new()
        {
            IAssetFilter filter = null;
            _editObjectService.Edit($"Add Filter {_commandId++}",
                () =>
                {
                    if (filter == null)
                        filter = _assetGroup.AddFilter<T>();
                    else
                        _assetGroup.AddFilter(filter);
                },
                () => _assetGroup.RemoveFilter(filter.Id));
        }

        public void AddFilter(Type type)
        {
            IAssetFilter filter = null;
            _editObjectService.Edit($"Add Filter {_commandId++}",
                () =>
                {
                    if (filter == null)
                        filter = _assetGroup.AddFilter(type);
                    else
                        _assetGroup.AddFilter(filter);
                },
                () => _assetGroup.RemoveFilter(filter.Id));
        }

        public void RemoveFilter(string filterId)
        {
            var oldFilter = _assetGroup.Filters[filterId];
            _editObjectService.Edit($"Remove Filter {filterId}",
                () => _assetGroup.RemoveFilter(filterId),
                () => _assetGroup.AddFilter(oldFilter));
        }

        public void MoveUpFilterOrder(string id)
        {
            var oldIndex = _assetGroup.GetFilterOrder(id);
            var newIndex = Mathf.Max(0, oldIndex - 1);

            if (oldIndex == newIndex)
                return;

            _editObjectService.Edit($"Set Filter Order {id}",
                () => _assetGroup.SetFilterOrder(id, newIndex),
                () => _assetGroup.SetFilterOrder(id, oldIndex));
        }

        public void MoveDownAssetGroupOrder(string id)
        {
            var oldIndex = _assetGroup.GetFilterOrder(id);
            var newIndex = Mathf.Min(oldIndex + 1, _assetGroup.Filters.Count - 1);

            if (oldIndex == newIndex)
                return;

            _editObjectService.Edit($"Set Filter Order {id}",
                () => _assetGroup.SetFilterOrder(id, newIndex),
                () => _assetGroup.SetFilterOrder(id, oldIndex));
        }

        public void OnMouseButtonClicked()
        {
            _mouseButtonClickedCount++;
        }

        public void SetupFilterHistory(string filterId)
        {
            var filter = _assetGroup.Filters[filterId];
            var filterHistory = new History(filter);
            filterHistory.RegisterSnapshot(filterHistory.TakeSnapshot());
            filterHistory.IncrementCurrentGroup();
            _perFilterStateBasedHistory.Add(filterId, filterHistory);
        }

        public void CleanupFilterHistory(string filterId)
        {
            _perFilterStateBasedHistory.Remove(filterId);
        }

        public void CleanupFilterHistories()
        {
            _perFilterStateBasedHistory.Clear();
        }

        public void RegisterFilterHistory(string filterId)
        {
            var filterHistory = _perFilterStateBasedHistory[filterId];
            var registered = filterHistory.RegisterSnapshot();
            if (!registered) return;

            // Set keyboardControl to commandName, so changes to the same control will be processed together.
            // But if the mouse button is clicked, commandName will be changed.
            // As a result, the undo for successive keyboard inputs will be processed at once and for mouse input is undo individually.
            filterHistory.IncrementCurrentGroup();
            _editObjectService.Edit($"On Filter Value Changed {GUIUtility.keyboardControl} {_mouseButtonClickedCount}",
                () => filterHistory.Redo(),
                () => filterHistory.Undo());
        }
    }
}

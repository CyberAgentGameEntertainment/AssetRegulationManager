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
            string filterId = null;
            _editObjectService.Edit($"Add Filter {_commandId++}",
                () =>
                {
                    var filter = _assetGroup.AddFilter<T>();
                    filterId = filter.Id;
                },
                () => _assetGroup.RemoveFilter(filterId));
        }

        public void AddFilter(Type type)
        {
            string filterId = null;
            _editObjectService.Edit($"Add Filter {_commandId++}",
                () =>
                {
                    var filter = _assetGroup.AddFilter(type);
                    filterId = filter.Id;
                },
                () => _assetGroup.RemoveFilter(filterId));
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

        public void OnFilterValueChanged(string filterId)
        {
            var filter = _assetGroup.Filters[filterId];
            if (!_perFilterStateBasedHistory.TryGetValue(filterId, out var history))
            {
                history = new History(filter);
                _perFilterStateBasedHistory.Add(filterId, history);
            }

            history.TakeSnapshot();
            history.IncrementCurrentGroup();

            _editObjectService.Edit($"On Filter Value Changed {filterId}",
                () => history.Redo(),
                () => history.Undo());
        }
    }
}

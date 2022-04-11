using System;
using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Shared;
using AssetRegulationManager.Editor.Foundation.StateBasedUndo;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.ApplicationService
{
    /// <summary>
    ///     Application Service to edit <see cref="AssetRegulation" />.
    /// </summary>
    internal sealed class EditAssetRegulationService
    {
        private readonly EditObjectService _editObjectService;

        private readonly Dictionary<string, History>
            _perConstraintStateBasedHistory = new Dictionary<string, History>();

        private readonly AssetRegulation _regulation;
        private int _commandId;

        public EditAssetRegulationService(AssetRegulation regulation, EditObjectService editObjectService)
        {
            _regulation = regulation;
            _editObjectService = editObjectService;
        }

        public void AddAssetGroup()
        {
            AssetGroup group = null;
            _editObjectService.Edit($"Add {nameof(AssetGroup)} {_commandId++}",
                () => group = _regulation.AddAssetGroup(),
                () => _regulation.RemoveAssetGroup(group.Id));
        }

        public void RemoveAssetGroup(string id)
        {
            AssetGroup group = null;
            _editObjectService.Edit($"Remove Target {id}",
                () =>
                {
                    group = _regulation.AssetGroups[id];
                    _regulation.RemoveAssetGroup(id);
                },
                () => _regulation.AddAssetGroup(group));
        }

        public void MoveUpAssetGroupOrder(string id)
        {
            var oldIndex = _regulation.GetAssetGroupOrder(id);
            var newIndex = Mathf.Max(0, oldIndex - 1);

            if (oldIndex == newIndex)
                return;

            _editObjectService.Edit($"Set Asset Group Order {id}",
                () => _regulation.SetAssetGroupOrder(id, newIndex),
                () => _regulation.SetAssetGroupOrder(id, oldIndex));
        }

        public void MoveDownAssetGroupOrder(string id)
        {
            var oldIndex = _regulation.GetAssetGroupOrder(id);
            var newIndex = Mathf.Min(oldIndex + 1, _regulation.AssetGroups.Count - 1);

            if (oldIndex == newIndex)
                return;

            _editObjectService.Edit($"Set Asset Group Order {id}",
                () => _regulation.SetAssetGroupOrder(id, newIndex),
                () => _regulation.SetAssetGroupOrder(id, oldIndex));
        }

        public void AddConstraint<T>() where T : IAssetConstraint, new()
        {
            string id = null;
            _editObjectService.Edit($"Add Constraint {_commandId++}",
                () =>
                {
                    var constraint = _regulation.AddConstraint<T>();
                    id = constraint.Id;
                },
                () => _regulation.RemoveConstraint(id));
        }

        public void AddConstraint(Type type)
        {
            string id = null;
            _editObjectService.Edit($"Add Constraint {_commandId++}",
                () =>
                {
                    var constraint = _regulation.AddConstraint(type);
                    id = constraint.Id;
                },
                () => _regulation.RemoveConstraint(id));
        }

        public void RemoveConstraint(string id)
        {
            var oldFilter = _regulation.Constraints[id];
            _editObjectService.Edit($"Remove Constraint {id}",
                () => _regulation.RemoveConstraint(id),
                () => _regulation.AddConstraint(oldFilter));
        }

        public void MoveUpConstraintOrder(string id)
        {
            var oldIndex = _regulation.GetConstraintOrder(id);
            var newIndex = Mathf.Max(0, oldIndex - 1);

            if (oldIndex == newIndex)
                return;

            _editObjectService.Edit($"Set Constraint Order {id}",
                () => _regulation.SetConstraintOrder(id, newIndex),
                () => _regulation.SetConstraintOrder(id, oldIndex));
        }

        public void MoveDownConstraintOrder(string id)
        {
            var oldIndex = _regulation.GetConstraintOrder(id);
            var newIndex = Mathf.Min(oldIndex + 1, _regulation.Constraints.Count - 1);

            if (oldIndex == newIndex)
                return;

            _editObjectService.Edit($"Set Constraint Order {id}",
                () => _regulation.SetConstraintOrder(id, newIndex),
                () => _regulation.SetConstraintOrder(id, oldIndex));
        }

        public void OnConstraintValueChanged(string id)
        {
            var constraint = _regulation.Constraints[id];
            if (!_perConstraintStateBasedHistory.TryGetValue(id, out var history))
            {
                history = new History(constraint);
                _perConstraintStateBasedHistory.Add(id, history);
            }

            history.TakeSnapshot();
            history.IncrementCurrentGroup();

            _editObjectService.Edit($"On Constraint Value Changed {id}",
                () => history.Redo(),
                () => history.Undo());
        }
    }
}

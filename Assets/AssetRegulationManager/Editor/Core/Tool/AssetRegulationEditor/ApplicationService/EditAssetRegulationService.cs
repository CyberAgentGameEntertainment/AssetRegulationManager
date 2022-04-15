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
        private int _mouseButtonClickedCount;

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

        public void CopyAssetGroup(string id)
        {
            var assetGroup = _regulation.AssetGroups[id];
            ObjectCopyBuffer.Register(assetGroup);
        }

        public bool CanPasteAssetGroup()
        {
            return ObjectCopyBuffer.Type == typeof(AssetGroup);
        }

        public void PasteAssetGroup()
        {
            AssetGroup group = null;
            var json = ObjectCopyBuffer.Json;
            _editObjectService.Edit($"Paste {nameof(AssetGroup)} {_commandId++}",
                () =>
                {
                    if (group == null)
                    {
                        group = _regulation.AddAssetGroup();
                        group.OverwriteValuesFromJson(json);
                    }
                    else
                    {
                        _regulation.AddAssetGroup(group);
                    }
                },
                () => _regulation.RemoveAssetGroup(group.Id));
        }

        public bool CanPasteAssetGroupValues()
        {
            return ObjectCopyBuffer.Type == typeof(AssetGroup);
        }

        public void PasteAssetGroupValues(string targetId)
        {
            var assetGroup = _regulation.AssetGroups[targetId];
            var oldJson = JsonUtility.ToJson(assetGroup);
            var json = ObjectCopyBuffer.Json;
            _editObjectService.Edit($"Paste {nameof(AssetGroup)} Values {_commandId++}",
                () => assetGroup.OverwriteValuesFromJson(json),
                () => assetGroup.OverwriteValuesFromJson(oldJson));
        }

        public IAssetConstraint AddConstraint<T>() where T : IAssetConstraint, new()
        {
            IAssetConstraint constraint = null;
            _editObjectService.Edit($"Add Constraint {_commandId++}",
                () =>
                {
                    if (constraint == null)
                        constraint = _regulation.AddConstraint<T>();
                    else
                        _regulation.AddConstraint(constraint);
                },
                () => _regulation.RemoveConstraint(constraint.Id));
            return constraint;
        }

        public IAssetConstraint AddConstraint(Type type)
        {
            IAssetConstraint constraint = null;
            _editObjectService.Edit($"Add Constraint {_commandId++}",
                () =>
                {
                    if (constraint == null)
                        constraint = _regulation.AddConstraint(type);
                    else
                        _regulation.AddConstraint(constraint);
                },
                () => _regulation.RemoveConstraint(constraint.Id));
            return constraint;
        }

        public void RemoveConstraint(string id)
        {
            var oldConstraint = _regulation.Constraints[id];
            _editObjectService.Edit($"Remove Constraint {id}",
                () => _regulation.RemoveConstraint(id),
                () => _regulation.AddConstraint(oldConstraint));
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

        public void OnMouseButtonClicked()
        {
            _mouseButtonClickedCount++;
        }

        public void SetupConstraintHistory(string constraintId)
        {
            var constraint = _regulation.Constraints[constraintId];
            var constraintHistory = new History(constraint);
            constraintHistory.RegisterSnapshot(constraintHistory.TakeSnapshot());
            constraintHistory.IncrementCurrentGroup();
            _perConstraintStateBasedHistory.Add(constraintId, constraintHistory);
        }

        public void CleanupConstraintHistory(string filterId)
        {
            _perConstraintStateBasedHistory.Remove(filterId);
        }

        public void CleanupConstraintHistories()
        {
            _perConstraintStateBasedHistory.Clear();
        }

        public void RegisterConstraintHistory(string constraintId)
        {
            var constraintHistory = _perConstraintStateBasedHistory[constraintId];
            var registered = constraintHistory.RegisterSnapshot();
            if (!registered) return;

            // Set keyboardControl to commandName, so changes to the same control will be processed together.
            // But if the mouse button is clicked, commandName will be changed.
            // As a result, the undo for successive keyboard inputs will be processed at once and for mouse input is undo individually.
            constraintHistory.IncrementCurrentGroup();
            _editObjectService.Edit($"On Filter Value Changed {GUIUtility.keyboardControl} {_mouseButtonClickedCount}",
                () => constraintHistory.Redo(),
                () => constraintHistory.Undo());
        }

        public void CopyConstraint(string id)
        {
            var constraint = _regulation.Constraints[id];
            ObjectCopyBuffer.Register(constraint);
        }

        public bool CanPasteConstraint()
        {
            return typeof(IAssetConstraint).IsAssignableFrom(ObjectCopyBuffer.Type);
        }

        public void PasteConstraint()
        {
            IAssetConstraint constraint = null;
            var type = ObjectCopyBuffer.Type;
            var json = ObjectCopyBuffer.Json;
            _editObjectService.Edit($"Paste Constraint {_commandId++}",
                () =>
                {
                    if (constraint == null)
                    {
                        constraint = _regulation.AddConstraint(type);
                        constraint.OverwriteValuesFromJson(json);
                    }
                    else
                    {
                        _regulation.AddConstraint(constraint);
                    }
                },
                () => _regulation.RemoveConstraint(constraint.Id));
        }

        public bool CanPasteConstraintValues(string targetId)
        {
            var constraint = _regulation.Constraints[targetId];
            return constraint.GetType() == ObjectCopyBuffer.Type;
        }

        public void PasteConstraintValues(string targetId)
        {
            var constraint = _regulation.Constraints[targetId];
            var oldJson = JsonUtility.ToJson(constraint);
            var json = ObjectCopyBuffer.Json;
            _editObjectService.Edit($"Paste Constraint Values {_commandId++}",
                () => constraint.OverwriteValuesFromJson(json),
                () => constraint.OverwriteValuesFromJson(oldJson));
        }
    }
}

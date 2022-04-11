using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Shared;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.ApplicationService
{
    /// <summary>
    ///     Application Service to edit <see cref="AssetRegulationSet" />.
    /// </summary>
    internal sealed class EditAssetRegulationSetService
    {
        private readonly AssetRegulationSet _regulations;
        private readonly EditObjectService _editStoreService;
        private int _commandId;

        public EditAssetRegulationSetService(AssetRegulationSet regulations, EditObjectService editStoreService)
        {
            _regulations = regulations;
            _editStoreService = editStoreService;
        }

        public AssetRegulation AddRegulation()
        {
            AssetRegulation regulation = null;
            _editStoreService.Edit($"Add {nameof(AssetRegulation)} {_commandId++}",
                () => regulation = _regulations.Add(),
                () => _regulations.Remove(regulation.Id));
            return regulation;
        }

        public void RemoveRegulations(IEnumerable<string> ids)
        {
            var regulations = new Dictionary<string, AssetRegulation>();
            var indices = new Dictionary<string, int>();
            var idArray = ids.ToArray();
            _editStoreService.Edit($"Remove {nameof(AssetRegulation)}s {_commandId++}",
                () =>
                {
                    foreach (var id in idArray)
                    {
                        var regulation = _regulations.Values[id];
                        var index = _regulations.GetIndex(id);
                        regulations.Add(id, regulation);
                        indices.Add(id, index);
                        _regulations.Remove(id);
                    }
                },
                () =>
                {
                    foreach (var id in idArray.Reverse())
                    {
                        var regulation = regulations[id];
                        var index = indices[id];
                        _regulations.Add(regulation);
                        _regulations.SetIndex(id, index);
                    }
                });
        }

        public void ChangeRuleIndex(string id, int index)
        {
            var oldIndex = _regulations.GetIndex(id);
            _editStoreService.Edit($"Change {nameof(AssetRegulation)} Index {_commandId++}",
                () => _regulations.SetIndex(id, index),
                () => _regulations.SetIndex(id, oldIndex));
        }

        public void ChangeRuleName(string id, string name)
        {
            var regulation = _regulations.Values[id];
            var oldValue = regulation.Name.Value;
            _editStoreService.Edit(
                $"Change {nameof(AssetRegulation)} Name {_commandId++}",
                () => regulation.Name.Value = name,
                () => regulation.Name.Value = oldValue);
        }
    }
}

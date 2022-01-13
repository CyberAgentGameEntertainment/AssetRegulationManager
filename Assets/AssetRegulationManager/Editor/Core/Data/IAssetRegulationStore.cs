using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;

namespace AssetRegulationManager.Editor.Core.Data
{
    public interface IAssetRegulationStore
    {
        IEnumerable<AssetRegulation> GetRegulations();
    }
}

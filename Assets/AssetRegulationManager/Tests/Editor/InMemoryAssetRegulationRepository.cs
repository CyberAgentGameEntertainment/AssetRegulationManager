using System.Collections.Generic;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;

namespace AssetRegulationManager.Tests.Editor
{
    internal sealed class InMemoryAssetRegulationRepository : IAssetRegulationRepository
    {
        public List<AssetRegulation> Regulations { get; } = new List<AssetRegulation>();

        public IEnumerable<AssetRegulation> GetAllRegulations()
        {
            return Regulations;
        }
    }
}

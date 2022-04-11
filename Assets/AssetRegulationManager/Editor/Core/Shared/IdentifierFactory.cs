using System;

namespace AssetRegulationManager.Editor.Core.Shared
{
    public static class IdentifierFactory
    {
        public static string Create()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

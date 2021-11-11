namespace AssetRegulationManager.Editor
{
    public interface IAssetRegulationEntry
    {
        string Label { get; }
        string Explanation { get; }
        bool RunTest(string path);
        void DrawGUI();
    }
}
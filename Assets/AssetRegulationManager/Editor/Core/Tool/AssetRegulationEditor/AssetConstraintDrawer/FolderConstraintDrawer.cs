using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl;
using AssetRegulationManager.Editor.Foundation.CustomDrawers;
using AssetRegulationManager.Editor.Foundation.ListableProperty;
using UnityEditor;

namespace AssetRegulationManager.Editor.Core.Tool.AssetRegulationEditor.AssetConstraintDrawer
{
    [CustomGUIDrawer(typeof(FolderConstraint))]
    internal class FolderConstraintDrawer : GUIDrawer<FolderConstraint>
    {
        private ObjectListablePropertyGUI _folderGUI;

        public override void Setup(object target)
        {
            base.Setup(target);
            _folderGUI = new ObjectListablePropertyGUI("Folder", Target.Folder, typeof(DefaultAsset), false);
        }

        protected override void GUILayout(FolderConstraint target)
        {
            target.CheckMode = (FolderConstraintCheckMode)EditorGUILayout.EnumPopup("Check Mode", target.CheckMode);
            target.TopFolderOnly = EditorGUILayout.Toggle("Top Folder Only", target.TopFolderOnly);
            _folderGUI.DoLayout();
        }
    }
}

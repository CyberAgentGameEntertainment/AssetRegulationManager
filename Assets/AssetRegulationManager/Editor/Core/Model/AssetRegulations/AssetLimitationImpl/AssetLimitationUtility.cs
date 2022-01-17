using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetLimitationImpl
{
    public static class AssetLimitationUtility
    {
        /// <summary>
        ///     Open scene.
        ///     If the active scene is dirty, show a save confirmation dialog.
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns>False if the user performs a cancel operation.</returns>
        public static bool OpenScene(string assetPath)
        {
            var isDirty = false;
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.isDirty)
                {
                    isDirty = true;
                    break;
                }
            }

            if (isDirty)
            {
                if (EditorUtility.DisplayDialog("Scene(s) Have Been Modified",
                        "Do you want to save the changes you made in the scenes?", "Save", "Cancel"))
                {
                    EditorSceneManager.SaveOpenScenes();
                }
                else
                {
                    return false;
                }
            }

            EditorSceneManager.OpenScene(assetPath);
            return true;
        }

        public static IEnumerable<T> GetAllComponentsInActiveScene<T>(bool includeInactive)
        {
            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            return rootGameObjects.SelectMany(x => x.GetComponentsInChildren<T>(includeInactive));
        }

        /// <summary>
        ///     Get Meshes from MeshFilter or SkinnedMeshRenderer.
        /// </summary>
        /// <param name="gameObj"></param>
        /// <param name="includeChildren"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static IEnumerable<Mesh> GetMeshesFromGameObject(GameObject gameObj, bool includeChildren,
            bool includeInactive)
        {
            if (includeChildren)
            {
                foreach (var meshFilter in gameObj.GetComponentsInChildren<MeshFilter>(includeInactive))
                {
                    yield return meshFilter.sharedMesh;
                }

                foreach (var skinnedMeshRenderer in gameObj.GetComponentsInChildren<SkinnedMeshRenderer>(
                             includeInactive))
                {
                    yield return skinnedMeshRenderer.sharedMesh;
                }
            }
            else
            {
                if (gameObj.TryGetComponent<MeshFilter>(out var meshFilter))
                {
                    yield return meshFilter.sharedMesh;
                }

                if (gameObj.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
                {
                    yield return skinnedMeshRenderer.sharedMesh;
                }
            }
        }
    }
}

// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Linq;
using UnityEditor;

namespace AssetRegulationManager.Tests.Editor
{
    public static class TestAssetRelativePaths
    {
        public const string DummyFolder = "Dummy";
        public const string TexelCountTestFolder = "TexelCountTest";

        public const string Texture64 = "tex_test_64.png";
        public const string Texture128 = "tex_test_128.png";
        public const string Texture128MaxSize64 = "tex_test_128_MaxSize64.png";
        public const string Texture64iOSAstc6AndAstc4 = "tex_test_64_iOSAstc6_AndAstc4.png";
        public const string Texture2048 = "tex_test_2048.png";
        public const string FbxSingleMesh = "fbx_test_singlemesh.fbx";
        public const string FbxMultiMesh = "fbx_test_multimesh.fbx";
        public const string Mesh24verts = "mesh_test_24verts.mesh";
        public const string PrefabSingleMesh = "prefab_test_singlemesh.prefab";
        public const string PrefabMultiMesh = "prefab_test_multimesh.prefab";
        public const string Prefab3Obj = "prefab_test_3obj.prefab";
        public const string PrefabTexel64x2And128 = "prefab_texel_64x2_and_128.prefab";
        public const string Scene3Obj = "scene_test_3obj.unity";
        public const string Prefab3Particles = "prefab_test_3particles.prefab";
        public const string Scene3Particles = "scene_test_3particles.unity";
        public const string Scene24x3Vertices = "scene_test_24x3vertices.unity";
        public const string SceneTexel64x2And128 = "scene_test_texel_64x2_and_128.unity";
        public const string MaterialTex64 = "mat_tex_64.mat";
        public const string MaterialTex64Duplicated = "mat_tex_64_duplicated.mat";
        public const string MaterialTex64MetallicParallelMap = "mat_tex_64_metallicparallaxmap.mat";
        public const string DummyPrefab = DummyFolder + "/Dummy.prefab";
    }

    public static class TestAssetPaths
    {
        private static string _folder;

        public static string Folder
        {
            get
            {
                if (!string.IsNullOrEmpty(_folder))
                    return _folder;
                var asmdefGuid = AssetDatabase.FindAssets("AssetRegulationManager.Tests.Editor").First();
                var asmdefPath = AssetDatabase.GUIDToAssetPath(asmdefGuid);
                var asmdefFolderPath = asmdefPath.Substring(0, asmdefPath.LastIndexOf("/", StringComparison.Ordinal));
                var baseFolderPath = $"{asmdefFolderPath}/TestAssets";
                return baseFolderPath;
            }
        }

        public static string DummyFolder => CreateAbsoluteAssetPath(TestAssetRelativePaths.DummyFolder);

        public static string TexelCountTestFolder =>
            CreateAbsoluteAssetPath(TestAssetRelativePaths.TexelCountTestFolder);

        public static string Texture64 => CreateAbsoluteAssetPath(TestAssetRelativePaths.Texture64);
        public static string Texture128 => CreateAbsoluteAssetPath(TestAssetRelativePaths.Texture128);
        public static string Texture128MaxSize64 => CreateAbsoluteAssetPath(TestAssetRelativePaths.Texture128MaxSize64);

        public static string Texture64iOSAstc6AndAstc4 =>
            CreateAbsoluteAssetPath(TestAssetRelativePaths.Texture64iOSAstc6AndAstc4);

        public static string Texture2048 => CreateAbsoluteAssetPath(TestAssetRelativePaths.Texture2048);
        public static string FbxSingleMesh => CreateAbsoluteAssetPath(TestAssetRelativePaths.FbxSingleMesh);
        public static string FbxMultiMesh => CreateAbsoluteAssetPath(TestAssetRelativePaths.FbxMultiMesh);
        public static string Mesh24verts => CreateAbsoluteAssetPath(TestAssetRelativePaths.Mesh24verts);
        public static string PrefabSingleMesh => CreateAbsoluteAssetPath(TestAssetRelativePaths.PrefabSingleMesh);
        public static string PrefabMultiMesh => CreateAbsoluteAssetPath(TestAssetRelativePaths.PrefabMultiMesh);
        public static string Prefab3Obj => CreateAbsoluteAssetPath(TestAssetRelativePaths.Prefab3Obj);

        public static string PrefabTexel64x2And128 =>
            CreateAbsoluteAssetPath(TestAssetRelativePaths.PrefabTexel64x2And128);

        public static string Scene3Obj => CreateAbsoluteAssetPath(TestAssetRelativePaths.Scene3Obj);
        public static string Prefab3Particles => CreateAbsoluteAssetPath(TestAssetRelativePaths.Prefab3Particles);
        public static string Scene3Particles => CreateAbsoluteAssetPath(TestAssetRelativePaths.Scene3Particles);
        public static string Scene24x3Vertices => CreateAbsoluteAssetPath(TestAssetRelativePaths.Scene24x3Vertices);

        public static string SceneTexel64x2And128 =>
            CreateAbsoluteAssetPath(TestAssetRelativePaths.SceneTexel64x2And128);

        public static string MaterialTex64 => CreateAbsoluteAssetPath(TestAssetRelativePaths.MaterialTex64);

        public static string MaterialTex64Duplicated =>
            CreateAbsoluteAssetPath(TestAssetRelativePaths.MaterialTex64Duplicated);

        public static string MaterialTex64MetallicParallelMap =>
            CreateAbsoluteAssetPath(TestAssetRelativePaths.MaterialTex64MetallicParallelMap);

        public static string DummyPrefab => CreateAbsoluteAssetPath(TestAssetRelativePaths.DummyPrefab);

        public static string CreateAbsoluteAssetPath(string relativeAssetPath)
        {
            if (string.IsNullOrEmpty(relativeAssetPath))
                return Folder;
            return $"{Folder}/{relativeAssetPath}";
        }
    }
}

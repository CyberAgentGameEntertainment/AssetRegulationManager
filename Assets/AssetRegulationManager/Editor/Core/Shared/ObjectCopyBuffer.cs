using System;
using UnityEngine;

namespace AssetRegulationManager.Editor.Core.Shared
{
    public static class ObjectCopyBuffer
    {
        public static string Json { get; private set; }

        public static Type Type { get; private set; }

        public static void Register(object target)
        {
            Type = target.GetType();
            Json = JsonUtility.ToJson(target);
        }
    }
}

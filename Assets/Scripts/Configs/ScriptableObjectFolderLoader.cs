using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BeaverBlocks.Configs
{
    public static class ScriptableObjectFolderLoader
    {
#if UNITY_EDITOR
        private const string PathToConfig = "Assets/Configs/";
        public static T[] MergeWithAssetsFromFolder<T>(T[] originalArray, string folderPath) where T : ScriptableObject
        {
            var originalSet = originalArray != null ? originalArray.ToHashSet() : new HashSet<T>();

            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { PathToConfig + folderPath });
            var newAssets = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path => AssetDatabase.LoadAssetAtPath<T>(path))
                .Where(asset => asset != null && !originalSet.Contains(asset));

            return originalSet.Concat(newAssets).ToArray();
        }
#endif
    }
}
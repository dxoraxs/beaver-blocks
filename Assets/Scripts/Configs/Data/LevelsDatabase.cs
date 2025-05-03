using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BeaverBlocks.Configs.Data
{
    [CreateAssetMenu(fileName = "levels_database", menuName = "Create/Configs/Data " + nameof(LevelsDatabase),
        order = 0)]
    public class LevelsDatabase : ScriptableObject
    {
        [SerializeField] private LevelConfig[] _levelConfigs;

        public IEnumerable<LevelConfig> LevelConfigs => _levelConfigs;

#if UNITY_EDITOR
        [ContextMenu("Fill LevelConfig from folder")]
        private void AddObjectBsFromFolder()
        {
            _levelConfigs = ScriptableObjectFolderLoader.MergeWithAssetsFromFolder(_levelConfigs, "Levels");
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
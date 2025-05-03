using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BeaverBlocks.Configs.Data
{
    [CreateAssetMenu(fileName = "blocks_database", menuName = "Create/Configs/Data " + nameof(BlocksDatabase),
        order = 0)]
    public class BlocksDatabase : ScriptableObject
    {
        [SerializeField] private BlockConfig[] _blockConfigs;

        public IEnumerable<BlockConfig> BlockConfigs => _blockConfigs;

#if UNITY_EDITOR
        [ContextMenu("Fill BlockConfig from folder")]
        private void AddObjectBsFromFolder()
        {
            _blockConfigs = ScriptableObjectFolderLoader.MergeWithAssetsFromFolder(_blockConfigs, "Blocks");
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
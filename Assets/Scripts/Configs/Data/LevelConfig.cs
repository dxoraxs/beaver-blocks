using System;
using UnityEngine;

namespace BeaverBlocks.Configs.Data
{
    [CreateAssetMenu(fileName = "level_config", menuName = "Create/Configs/Data " + nameof(LevelConfig),
        order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public BlockConfig[] InitialBlocks { get; private set; }
        [field: SerializeField] public PrePlacedCell[] PrePlacedCells { get; private set; }
        
        public void SetCells(PrePlacedCell[] cells)
        {
            PrePlacedCells = cells;
        }
    }

    [Serializable]
    public class PrePlacedCell
    {
        [field:SerializeField] public Vector2Int Position { get; private set; }
        [field:SerializeField] public int GroupIndex { get; private set; }
        
        public PrePlacedCell(Vector2Int pos, int group)
        {
            Position = pos;
            GroupIndex = group;
        }
    }
}
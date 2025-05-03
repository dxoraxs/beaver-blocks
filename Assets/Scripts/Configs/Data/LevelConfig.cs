using UnityEngine;

namespace BeaverBlocks.Configs.Data
{
    [CreateAssetMenu(fileName = "level_config", menuName = "Create/Configs/Data " + nameof(LevelConfig),
        order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public BlockConfig[] InitialBlocks { get; private set; }
        [field: SerializeField] public Vector2Int[] PrePlacedPositions { get; private set; }
    }
}
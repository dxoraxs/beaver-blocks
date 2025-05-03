using UnityEngine;

namespace BeaverBlocks.Configs.Data
{
    [CreateAssetMenu(fileName = "game_settings", menuName = "Create/Configs/Data " + nameof(GameSettings),
        order = 0)]
    public class GameSettings : ScriptableObject
    {
        [field: SerializeField] public int GridSizeX { get; private set; } = 8;
        [field: SerializeField] public int GridSizeY { get; private set; } = 8;
        [field: SerializeField] public int PointsPerCube { get; private set; } = 10;
    }
}
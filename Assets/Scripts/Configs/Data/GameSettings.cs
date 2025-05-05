using UnityEngine;

namespace BeaverBlocks.Configs.Data
{
    [CreateAssetMenu(fileName = "game_settings", menuName = "Create/Configs/Data " + nameof(GameSettings),
        order = 0)]
    public class GameSettings : ScriptableObject
    {
        [field: SerializeField] public uint GridSize { get; private set; } = 8;
        [field: SerializeField] public uint PointsPerCube { get; private set; } = 10;
        [field: SerializeField] public uint CountBottomPlace { get; private set; } = 3;
        [field: SerializeField] public float DragVerticalOffset { get; private set; } = 200;
        [field: SerializeField] public float SpeedDragBlock { get; private set; } = 2;
    }
}
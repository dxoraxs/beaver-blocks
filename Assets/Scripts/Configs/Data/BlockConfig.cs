using System;
using UnityEngine;

namespace BeaverBlocks.Configs.Data
{
    [CreateAssetMenu(fileName = "block_config", menuName = "Create/Configs/Data " + nameof(BlockConfig),
        order = 0)]
    public class BlockConfig : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public Vector2Int[] Shape { get; private set; }
        
        public void SetShape(Vector2Int[] newShape)
        {
            Shape = newShape;
        }
    }
}
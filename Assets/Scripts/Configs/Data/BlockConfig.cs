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
        [field: SerializeField] public Vector2 CenterOffset { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        
        public void SetShape(Vector2Int[] newShape)
        {
            Shape = newShape;
            CenterOffset = CalculateCenterOffset(newShape);
        }
        
        private Vector2 CalculateCenterOffset(Vector2Int[] shape)
        {
            if (shape == null || shape.Length == 0)
                return Vector2.zero;

            var min = shape[0];
            var max = shape[0];

            foreach (var cell in shape)
            {
                min = Vector2Int.Min(min, cell);
                max = Vector2Int.Max(max, cell);
            }

            var center = (min + max + Vector2.one) * 0.5f;
            return center;
        }
    }
}
using System;
using UnityEngine;

namespace BeaverBlocks.Configs.Data
{
    [CreateAssetMenu(fileName = "cell_colors_config", menuName = "Create/Configs/Data " + nameof(CellColorsConfig),
        order = 0)]
    public class CellColorsConfig : ScriptableObject
    {
        [field:SerializeField] public PairColorValue[] CellColors { get; private set; }
    }

    [Serializable]
    public class PairColorValue
    {
        [field:SerializeField] public Color DefaultColors { get; private set; }
        [field:SerializeField] public Color PreviewColors { get; private set; }
    }
}
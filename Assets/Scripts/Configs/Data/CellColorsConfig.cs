using UnityEngine;

namespace BeaverBlocks.Configs.Data
{
    [CreateAssetMenu(fileName = "cell_colors_config", menuName = "Create/Configs/Data " + nameof(CellColorsConfig),
        order = 0)]
    public class CellColorsConfig : ScriptableObject
    {
        [field:SerializeField] public Color[] CellColors { get; private set; }
    }
}
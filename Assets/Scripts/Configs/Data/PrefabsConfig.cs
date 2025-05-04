using BeaverBlocks.Core.Cells;
using BeaverBlocks.UI.Views.Game.Cells;
using UnityEngine;

namespace BeaverBlocks.Configs.Data
{
    [CreateAssetMenu(fileName = "prefabs_config", menuName = "Create/Configs/Data " + nameof(PrefabsConfig),
        order = 0)]
    public class PrefabsConfig : ScriptableObject
    {
        [field: SerializeField] public CellView CellView { get; private set; }
    }
}
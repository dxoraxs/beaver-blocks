using System.Linq;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Cells;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Game.Level
{
    public class CellLevelInstaller
    {
        private readonly CellModelManager _cellModelManager;

        [Preserve]
        public CellLevelInstaller(CellModelManager cellModelManager, IConfigsService configsService)
        {
            _cellModelManager = cellModelManager;
        }

        public void Install(LevelConfig levelConfig)
        {
            var preLoadCells = levelConfig.PrePlacedCells;

            foreach (var prePlacedCell in preLoadCells)
            {
                var cellKey = (prePlacedCell.Position.x,prePlacedCell.Position.y);
                _cellModelManager.CellModels[cellKey].SetBusy(prePlacedCell.GroupIndex);
            }
        }
    }
}
using System.Linq;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Cells;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Game.Level
{
    public class CellLevelInstaller
    {
        private readonly CellsManager _cellsManager;
        private readonly CellColorsConfig _colorsConfig;

        [Preserve]
        public CellLevelInstaller(CellsManager cellsManager, IConfigsService configsService)
        {
            _cellsManager = cellsManager;
            _colorsConfig = configsService.Get<CellColorsConfig>();
        }

        public void Install(LevelConfig levelConfig)
        {
            var preLoadCells = levelConfig.PrePlacedCells;

            foreach (var prePlacedCell in preLoadCells)
            {
                var cellKey = (prePlacedCell.Position.x,prePlacedCell.Position.y);
                var currentColor = _colorsConfig.CellColors[prePlacedCell.GroupIndex];
                _cellsManager.CellPresenters[cellKey].SetBlockColor(currentColor);
            }
        }
    }
}
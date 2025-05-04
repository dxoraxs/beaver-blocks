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
        private readonly IConfigsService _configsService;

        [Preserve]
        public CellLevelInstaller(CellsManager cellsManager, IConfigsService configsService)
        {
            _cellsManager = cellsManager;
            _configsService = configsService;
        }

        public void Install(uint levelIndex)
        {
            var allLevels = _configsService.Get<LevelsDatabase>().LevelConfigs.ToArray();
            var currentLevel = allLevels[levelIndex % allLevels.Length];
            var preLoadCells = currentLevel.PrePlacedCells;

            foreach (var prePlacedCell in preLoadCells)
            {
                var cellKey = (prePlacedCell.Position.x,prePlacedCell.Position.y);
                var currentColor = _configsService.Get<CellColorsConfig>().CellColors[prePlacedCell.GroupIndex];
                _cellsManager.CellPresenters[cellKey].SetBlockColor(currentColor);
            }
        }
    }
}
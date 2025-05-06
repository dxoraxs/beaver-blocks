using System.Linq;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Cells;
using BeaverBlocks.Core.Game.BlockPlace;
using BeaverBlocks.DI.Factories;
using UnityEngine;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Game.Level
{
    public class InitialLevelBlockInstaller
    {
        private readonly BlockPlaceModelManager _blockPlaceModelManager;
        private readonly uint _countBottomPlace;
        private readonly CellColorsConfig _colorsConfig;

        [Preserve]
        public InitialLevelBlockInstaller(BlockPlaceModelManager blockPlaceModelManager, IConfigsService configsService)
        {
            _blockPlaceModelManager = blockPlaceModelManager;
            _countBottomPlace = configsService.Get<GameSettings>().CountBottomPlace;
            _colorsConfig = configsService.Get<CellColorsConfig>();
        }

        public void Install(LevelConfig levelConfig)
        {
            var initialBlocks = levelConfig.InitialBlocks;

            for (var index = 0u; index < initialBlocks.Length && index < _countBottomPlace; index++)
            {
                var currentBlock = initialBlocks[index];
                if (currentBlock == null)
                {
                    continue;
                }
                
                var randomColor = Random.Range(0, _colorsConfig.CellColors.Length);
                _blockPlaceModelManager.PlaceModels[index].SetBlock(currentBlock.Id, randomColor);
            }
        }
    }
}
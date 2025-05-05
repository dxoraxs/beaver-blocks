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
        private readonly BlockPlaceManager _blockPlaceManager;
        private readonly uint _countBottomPlace;
        private readonly CellColorsConfig _colorsConfig;

        [Preserve]
        public InitialLevelBlockInstaller(BlockPlaceManager blockPlaceManager, IConfigsService configsService)
        {
            _blockPlaceManager = blockPlaceManager;
            _countBottomPlace = configsService.Get<GameSettings>().CountBottomPlace;
            _colorsConfig = configsService.Get<CellColorsConfig>();
        }

        public void Install(LevelConfig levelConfig)
        {
            var initialBlocks = levelConfig.InitialBlocks;

            for (var index = 0u; index < initialBlocks.Length && index < _countBottomPlace; index++)
            {
                var currentBlockSprite = initialBlocks[index].Sprite;
                var randomColor = _colorsConfig.CellColors[Random.Range(0, _colorsConfig.CellColors.Length)].DefaultColors;
                _blockPlaceManager.BlockPlacePresenters[index].SetSprite(currentBlockSprite);
                _blockPlaceManager.BlockPlacePresenters[index].SetColor(randomColor);
            }
        }
    }
}
using System;
using System.Linq;
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Cells;
using BeaverBlocks.Core.Game.BlockPlace;
using BeaverBlocks.Core.Game.Level;
using BeaverBlocks.DI.Factories;
using BeaverBlocks.UI;
using BeaverBlocks.UI.Views.Game;
using Cysharp.Threading.Tasks;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Game
{
    public class GameplayModeController
    {
        private readonly IIocFactory _iocFactory;
        private readonly IConfigsService _configsService;
        private readonly IPanelService _panelService;
        private readonly CellModelManager _cellModelManager;
        private readonly CellPresenterManager _cellPresenterManager;
        private readonly BlockPlaceManager _blockPlaceManager;

        private readonly LevelsDatabase _levelsDatabase;
        private uint _levelIndex;

        private GameView GameView => _panelService.Get<GameView>();
        
        [Preserve]
        public GameplayModeController(IConfigsService configsService, IPanelService panelService, IIocFactory iocFactory, CellPresenterManager cellPresenterManager, BlockPlaceManager blockPlaceManager, CellModelManager cellModelManager)
        {
            _configsService = configsService;
            _panelService = panelService;
            _iocFactory = iocFactory;
            _cellPresenterManager = cellPresenterManager;
            _blockPlaceManager = blockPlaceManager;
            _cellModelManager = cellModelManager;

            _levelsDatabase = _configsService.Get<LevelsDatabase>();

            Initialize();
        }

        private void Initialize()
        {
            InitializeCells();
            InitializeBlockPlaces();
            
            GameView.Initialize(_iocFactory.Create<GamePresenter>());
            GameView.SetEnabled(true);

            StartLevel().Forget();
        }

        private void InitializeCells()
        {
            var cellModelInstaller = new CellModelInstaller(_configsService.Get<GameSettings>().GridSize);
            cellModelInstaller.Initialize();
            _cellModelManager.SetModels(cellModelInstaller.CellModels);
            
            var cellPresentersInstaller = new CellPresentersInstaller(_configsService.Get<GameSettings>().GridSize);
            cellPresentersInstaller.Initialize();
            _cellPresenterManager.SetCells(cellPresentersInstaller.CellPresenters, _cellModelManager.CellModels.Values);
        }

        private void InitializeBlockPlaces()
        {
            var blockPlaceInstaller = new BlockPlaceInstaller(_configsService.Get<GameSettings>().CountBottomPlace);
            blockPlaceInstaller.Initialize();
            _blockPlaceManager.SetBlockPlaces(blockPlaceInstaller.BlockPlacePresenters);
        }

        private async UniTask StartLevel()
        {
            var currentLevel = GetLevel();

            var cellLevelInstaller = _iocFactory.Create<CellLevelInstaller>();
            cellLevelInstaller.Install(currentLevel);

            await UniTask.Delay(TimeSpan.FromSeconds(1));
            
            var initialLevelBlockPlaceInstaller = _iocFactory.Create<InitialLevelBlockInstaller>();
            initialLevelBlockPlaceInstaller.Install(currentLevel);
        }

        private LevelConfig GetLevel()
        {
            var allLevel = _levelsDatabase.LevelConfigs.ToArray();
            var currentLevel = allLevel[_levelIndex % allLevel.Length];
            return currentLevel;
        }
    }
}
using BeaverBlocks.Configs;
using BeaverBlocks.Configs.Data;
using BeaverBlocks.Core.Cells;
using BeaverBlocks.DI.Factories;
using BeaverBlocks.UI;
using BeaverBlocks.UI.Views.Game;
using UnityEngine.Scripting;

namespace BeaverBlocks.Core.Game
{
    public class GameplayModeController
    {
        private readonly IIocFactory _iocFactory;
        private readonly IConfigsService _configsService;
        private readonly IPanelService _panelService;
        private readonly CellsManager _cellsManager;

        private GameView GameView => _panelService.Get<GameView>();
        
        [Preserve]
        public GameplayModeController(IConfigsService configsService, IPanelService panelService, IIocFactory iocFactory, CellsManager cellsManager)
        {
            _configsService = configsService;
            _panelService = panelService;
            _iocFactory = iocFactory;
            _cellsManager = cellsManager;

            Initialize();
        }

        private void Initialize()
        {
            InitializeCells();
        }

        private void InitializeCells()
        {
            var cellsInstaller = new CellsInstaller(_configsService.Get<GameSettings>().GridSize);
            cellsInstaller.Initialize();
            _cellsManager.SetCells(cellsInstaller.CellPresenters);
            
            GameView.Initialize(_iocFactory.Create<GamePresenter>());
            GameView.SetEnabled(true);
        }

        private void StartLevel()
        {
            
        }
    }
}
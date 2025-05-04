using BeaverBlocks.Configs;
using BeaverBlocks.DI.Factories;
using BeaverBlocks.UI;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace BeaverBlocks.Core.Game
{
    public class GameController : IInitializable
    {
        private readonly IIocFactory _iocFactory;
        private readonly IPanelService _panelService;
        private readonly IConfigsService _configsService;
        
        private GameplayModeController _gameplayModeController;

        [Preserve]
        public GameController(IIocFactory iocFactory, IPanelService panelService, IConfigsService configsService)
        {
            _iocFactory = iocFactory;
            _panelService = panelService;
            _configsService = configsService;
        }

        public void Initialize()
        {
            _panelService.Initialize();
            _configsService.Initialize();

            _gameplayModeController = _iocFactory.Create<GameplayModeController>();
        }
    }
}
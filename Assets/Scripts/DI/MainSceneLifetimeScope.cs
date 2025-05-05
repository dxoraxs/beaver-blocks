using BeaverBlocks.Configs;
using BeaverBlocks.Core.Cells;
using BeaverBlocks.Core.Game;
using BeaverBlocks.Core.Game.BlockPlace;
using BeaverBlocks.DI.Factories;
using BeaverBlocks.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

namespace BeaverBlocks.DI
{
    public class MainSceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private ConfigsService _configsService;
        [SerializeField] private PanelService _panelService;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<VContainerFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_configsService).As<IConfigsService>();
            builder.RegisterInstance(_panelService).As<IPanelService>();
            builder.RegisterEntryPoint<InputController>().As<IInputController>();
            
            builder.RegisterEntryPoint<GameController>(Lifetime.Scoped).AsSelf();
        }
    }
}
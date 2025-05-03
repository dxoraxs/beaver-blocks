using BeaverBlocks.DI.Factories;
using BeaverBlocks.Initialization;
using VContainer;
using VContainer.Unity;

namespace BeaverBlocks.DI
{
    public class InitSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InitializationController>();
        }
    }
}
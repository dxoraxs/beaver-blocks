using BeaverBlocks.DI.Factories;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BeaverBlocks.DI
{
    public class MainSceneLifetimeScope : LifetimeScope
    {

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<VContainerFactory>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
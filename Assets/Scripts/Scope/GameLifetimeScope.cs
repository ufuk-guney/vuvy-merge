using UnityEngine;
using VContainer;
using VContainer.Unity;
using VuvyMerge.Data;
using VuvyMerge.UI;

namespace VuvyMerge
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private BoardItemConfig _boardItemConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<HomeScreen>().As<IScreen>();
            builder.RegisterComponentInHierarchy<InGameScreen>().As<IScreen>();

            builder.RegisterEntryPoint<ScreenManager>();

            builder.RegisterInstance(_boardItemConfig);
            builder.RegisterEntryPoint<GridScope>();
        }
    }
}

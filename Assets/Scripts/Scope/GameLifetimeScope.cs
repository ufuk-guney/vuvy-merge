using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private BoardItemConfig _boardItemDatabase;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<HomeScreen>().As<IScreen>();
        builder.RegisterComponentInHierarchy<InGameScreen>().As<IScreen>();

        builder.RegisterEntryPoint<ScreenManager>();

        builder.RegisterInstance(_boardItemDatabase);
        builder.RegisterEntryPoint<GridScope>();
    }
}

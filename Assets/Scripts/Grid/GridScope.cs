using System;
using VContainer;
using VContainer.Unity;

public class GridScope : IInitializable, IDisposable
{
    private readonly LifetimeScope _parentScope;
    private LifetimeScope _childScope;

    public GridScope(LifetimeScope parentScope)
    {
        _parentScope = parentScope;
    }

    public void Initialize()
    {
        EventManager.Subscribe(EventType.OnLevelStartClick, OnLevelStart);
        EventManager.Subscribe(EventType.OnReturnHomeClick, OnReturnHome);
    }
    private void OnLevelStart()
    {
        _childScope?.Dispose();

        _childScope = _parentScope.CreateChild(builder =>
        {
            builder.Register<TileHandler>(Lifetime.Scoped);
            builder.Register<GridStateManager>(Lifetime.Scoped);

            builder.Register<DragHandler>(Lifetime.Scoped);
            builder.Register<DropHandler>(Lifetime.Scoped);
            builder.Register<MergeHandler>(Lifetime.Scoped);

            builder.RegisterEntryPoint<GridInitializer>();
            builder.RegisterEntryPoint<ItemFactory>().AsSelf();
            builder.RegisterEntryPoint<GridInputController>();
        });
    }

    private void OnReturnHome()
    {
        _childScope?.Dispose();
        _childScope = null;
    }

    public void Dispose()
    {
        EventManager.Unsubscribe(EventType.OnLevelStartClick, OnLevelStart);
        EventManager.Unsubscribe(EventType.OnReturnHomeClick, OnReturnHome);
        _childScope?.Dispose();
    }
}

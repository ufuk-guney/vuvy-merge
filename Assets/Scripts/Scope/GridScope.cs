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
        EventBus.Subscribe(EventType.OnLevelStartClick, OnLevelStart);
        EventBus.Subscribe(EventType.OnReturnHomeClick, OnReturnHome);
    }
    private void OnLevelStart()
    {
        _childScope?.Dispose();

        _childScope = _parentScope.CreateChild(builder =>
        {
            builder.RegisterEntryPoint<GridController>(Lifetime.Scoped)
                .As<IGridReader>()
                .As<IGridWriter>()
                .As<IGridHighlighter>();

            builder.Register<DragHandler>(Lifetime.Scoped);
            builder.Register<DropHandler>(Lifetime.Scoped);
            builder.Register<MergeHandler>(Lifetime.Scoped);

            builder.RegisterEntryPoint<ItemFactory>(Lifetime.Scoped).AsSelf().As<IItemSpawner>();
            builder.RegisterEntryPoint<GridInputController>(Lifetime.Scoped);
        });
    }

    private void OnReturnHome()
    {
        _childScope?.Dispose();
        _childScope = null;
    }

    public void Dispose()
    {
        EventBus.Unsubscribe(EventType.OnLevelStartClick, OnLevelStart);
        EventBus.Unsubscribe(EventType.OnReturnHomeClick, OnReturnHome);
        _childScope?.Dispose();
    }
}

using System;
using VContainer;
using VContainer.Unity;
using VuvyMerge.Grid;

namespace VuvyMerge
{
    public class GridScope : IInitializable, IDisposable
    {
        private readonly LifetimeScope _lifetimeScope;
        private LifetimeScope _gridScope;

        public GridScope(LifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public void Initialize()
        {
            EventBus.Subscribe(EventType.OnLevelStartClick, OnLevelStart);
            EventBus.Subscribe(EventType.OnReturnHomeClick, OnReturnHome);
        }

        private void OnLevelStart()
        {
            _gridScope?.Dispose();

            _gridScope = _lifetimeScope.CreateChild(builder =>
            {
                builder.RegisterEntryPoint<GridController>(Lifetime.Scoped)
                    .As<IGridReader>()
                    .As<IGridWriter>()
                    .As<IGridHighlighter>();

                builder.Register<DragService>(Lifetime.Scoped).As<IInputHandler>();
                builder.Register<DropService>(Lifetime.Scoped);
                builder.Register<MergeService>(Lifetime.Scoped);

                builder.RegisterEntryPoint<ItemFactory>(Lifetime.Scoped).AsSelf().As<IItemSpawner>();
                builder.RegisterEntryPoint<GridInputController>(Lifetime.Scoped);
            });
        }

        private void OnReturnHome()
        {
            _gridScope?.Dispose();
            _gridScope = null;
        }

        public void Dispose()
        {
            EventBus.Unsubscribe(EventType.OnLevelStartClick, OnLevelStart);
            EventBus.Unsubscribe(EventType.OnReturnHomeClick, OnReturnHome);
            _gridScope?.Dispose();
        }
    }
}

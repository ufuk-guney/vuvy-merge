using VContainer.Unity;

public class GridInitializer : IInitializable
{
    private readonly TileHandler _tileGenerator;
    private readonly GridStateManager _gridStateManager;
    private readonly ObjectPool<GridItem> _itemPool;
    private readonly LifetimeScope _scope;

    public GridInitializer(
        TileHandler tileGenerator,
        GridStateManager gridStateManager,
        ObjectPool<GridItem> itemPool,
        LifetimeScope scope)
    {
        _tileGenerator = tileGenerator;
        _gridStateManager = gridStateManager;
        _itemPool = itemPool;
        _scope = scope;
    }

    public void Initialize()
    {
        var gridData = new GridData(5, 5);
        _tileGenerator.GenerateTiles(gridData, _scope.transform);
        _gridStateManager.Initialize(gridData);
        _itemPool.Prewarm();
    }
}

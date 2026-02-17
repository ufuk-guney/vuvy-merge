using VContainer.Unity;

public class GridInitializer : IInitializable
{
    private readonly TileHandler _tileGenerator;
    private readonly GridStateManager _gridStateManager;
    private readonly LifetimeScope _scope;

    public GridInitializer(
        TileHandler tileGenerator,
        GridStateManager gridStateManager,
        LifetimeScope scope)
    {
        _tileGenerator = tileGenerator;
        _gridStateManager = gridStateManager;
        _scope = scope;
    }

    public void Initialize()
    {
        var gridData = new GridData(Constants.Grid.Width, Constants.Grid.Height);
        _tileGenerator.GenerateTiles(gridData, _scope.transform);
        _gridStateManager.Initialize(gridData);
    }
}

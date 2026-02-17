using UnityEngine;

public class MergeHandler
{
    private readonly GridStateManager _gridState;
    private readonly ItemFactory _itemFactory;
    private readonly BoardItemConfig _database;

    public MergeHandler(GridStateManager gridState, ItemFactory itemFactory, BoardItemConfig database)
    {
        _gridState = gridState;
        _itemFactory = itemFactory;
        _database = database;
    }

    public bool TryMerge(Vector2Int dragPos, Vector2Int dropPos)
    {
        var dragData = _gridState.GetDataAt(dragPos);
        var dropData = _gridState.GetDataAt(dropPos);
        if (!dragData.HasValue || !dropData.HasValue) return false;

        if (!dragData.Value.CanMerge(dropData.Value, _database))
        {
            Debug.Log($"Merge yapılamaz: {dragData.Value.ChainType}");
            return false;
        }

        int nextLevel = dragData.Value.Level + 1;

        var dragView = _gridState.GetViewAt(dragPos);
        var dropView = _gridState.GetViewAt(dropPos);

        _gridState.RemoveItem(dragPos);
        _gridState.RemoveItem(dropPos);

        _itemFactory.ReturnView(dragView);
        _itemFactory.ReturnView(dropView);

        _itemFactory.SpawnItem(dragData.Value.ChainType, nextLevel, dropPos);

        int score = (nextLevel) * 10;
        EventManager.Trigger(EventType.OnMerge, score);

        return true;
    }
}

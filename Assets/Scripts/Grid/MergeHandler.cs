public class MergeHandler
{
    private readonly GridStateManager _gridState;
    private readonly IItemSpawner _itemSpawner;
    private readonly BoardItemConfig _database;

    public MergeHandler(GridStateManager gridState, IItemSpawner itemSpawner, BoardItemConfig database)
    {
        _gridState = gridState;
        _itemSpawner = itemSpawner;
        _database = database;
    }

    public bool TryMerge(SlotPosition dragPos, SlotPosition dropPos)
    {
        var dragSlot = _gridState.GetSlotAt(dragPos);
        var dropSlot = _gridState.GetSlotAt(dropPos);
        if (!dragSlot.Data.HasValue || !dropSlot.Data.HasValue) return false;

        var dragData = dragSlot.Data.Value;
        var dropData = dropSlot.Data.Value;

        if (!dragData.CanMerge(dropData)) return false;

        if (!dragData.CanMerge(dropData, _database))
        {
            EventManager.Trigger<string>(EventType.OnWarning, "Max level!");
            return false;
        }

        int nextLevel = dragData.Level + 1;

        _gridState.RemoveItem(dragPos);
        _gridState.RemoveItem(dropPos);

        _itemSpawner.ReturnView(dragSlot.View);
        _itemSpawner.ReturnView(dropSlot.View);

        _itemSpawner.SpawnItem(dragData.ChainType, nextLevel, dropPos);

        int score = nextLevel * Constants.Scoring.ScorePerLevel;
        EventManager.Trigger(EventType.OnMerge, score);

        return true;
    }
}

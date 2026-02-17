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

        _itemFactory.ReturnView(dragSlot.View);
        _itemFactory.ReturnView(dropSlot.View);

        _itemFactory.SpawnItem(dragData.ChainType, nextLevel, dropPos);

        int score = nextLevel * Constants.Scoring.ScorePerLevel;
        EventManager.Trigger(EventType.OnMerge, score);

        return true;
    }
}

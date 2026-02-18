public class MergeHandler
{
    private readonly IGridReader _gridReader;
    private readonly IGridWriter _gridWriter;
    private readonly IItemSpawner _itemSpawner;
    private readonly BoardItemConfig _database;

    public MergeHandler(IGridReader gridReader, IGridWriter gridWriter, IItemSpawner itemSpawner, BoardItemConfig database)
    {
        _gridReader = gridReader;
        _gridWriter = gridWriter;
        _itemSpawner = itemSpawner;
        _database = database;
    }

    public bool TryMerge(SlotPosition dragPos, SlotPosition dropPos)
    {
        var dragSlot = _gridReader.GetSlotAt(dragPos);
        var dropSlot = _gridReader.GetSlotAt(dropPos);
        if (!dragSlot.Data.HasValue || !dropSlot.Data.HasValue) return false;

        var dragData = dragSlot.Data.Value;
        var dropData = dropSlot.Data.Value;

        if (!dragData.CanMerge(dropData)) return false;

        var chainData = _database.ItemChainDataList.Find(c => c.ChainType == dragData.ChainType);
        if (chainData == null) return false;

        if (!dragData.CanMerge(dropData, chainData.MaxLevel))
        {
            EventManager.Trigger<string>(EventType.OnWarning, "Max level!");
            return false;
        }

        int nextLevel = dragData.Level + 1;

        var dragView = _gridReader.GetItemViewAt(dragPos);
        var dropView = _gridReader.GetItemViewAt(dropPos);

        _gridWriter.RemoveItem(dragPos);
        _gridWriter.RemoveItem(dropPos);

        _itemSpawner.ReturnView(dragView);
        _itemSpawner.ReturnView(dropView);

        _itemSpawner.SpawnItem(dragData.ChainType, nextLevel, dropPos);

        int score = nextLevel * Constants.Scoring.ScorePerLevel;
        EventManager.Trigger(EventType.OnMerge, score);

        return true;
    }
}

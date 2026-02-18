using System.Collections.Generic;

public class MergeHandler
{
    private readonly IGridReader _gridReader;
    private readonly IGridWriter _gridWriter;
    private readonly IItemSpawner _itemSpawner;
    private readonly Dictionary<ItemChainType, ItemChainData> _chainLookup;

    public MergeHandler(IGridReader gridReader, IGridWriter gridWriter, IItemSpawner itemSpawner, BoardItemConfig database)
    {
        _gridReader = gridReader;
        _gridWriter = gridWriter;
        _itemSpawner = itemSpawner;
        _chainLookup = BuildChainLookup(database.ItemChainDataList);
    }

    public bool TryMerge(SlotPosition dragPos, SlotPosition dropPos)
    {
        var dragSlot = _gridReader.GetSlotAt(dragPos);
        var dropSlot = _gridReader.GetSlotAt(dropPos);
        if (!dragSlot.Data.HasValue || !dropSlot.Data.HasValue) return false;

        var dragData = dragSlot.Data.Value;
        var dropData = dropSlot.Data.Value;

        if (!TryGetNextLevel(dragData, dropData, out int nextLevel)) return false;

        ExecuteMerge(dragPos, dropPos, dragData.ChainType, nextLevel);
        return true;
    }

    private bool TryGetNextLevel(ItemData dragData, ItemData dropData, out int nextLevel)
    {
        nextLevel = 0;

        if (!dragData.CanMerge(dropData)) return false;
        if (!_chainLookup.TryGetValue(dragData.ChainType, out var chainData)) return false;

        if (!dragData.CanMerge(dropData, chainData.MaxLevel))
        {
            EventBus.Trigger<string>(EventType.OnWarning, Constants.Text.MaxLevelWarning);
            return false;
        }

        nextLevel = dragData.Level + 1;
        return true;
    }

    private void ExecuteMerge(SlotPosition dragPos, SlotPosition dropPos, ItemChainType chainType, int nextLevel)
    {
        var dragView = _gridReader.GetItemViewAt(dragPos);
        var dropView = _gridReader.GetItemViewAt(dropPos);

        _gridWriter.RemoveItem(dragPos);
        _gridWriter.RemoveItem(dropPos);

        _itemSpawner.ReturnView(dragView);
        _itemSpawner.ReturnView(dropView);

        _itemSpawner.SpawnItem(chainType, nextLevel, dropPos);

        EventBus.Trigger(EventType.OnMerge, nextLevel * Constants.Scoring.ScorePerLevel);
    }

    private static Dictionary<ItemChainType, ItemChainData> BuildChainLookup(List<ItemChainData> chainDataList)
    {
        var lookup = new Dictionary<ItemChainType, ItemChainData>(chainDataList.Count);
        foreach (var chainData in chainDataList)
            lookup[chainData.ChainType] = chainData;
        return lookup;
    }
}

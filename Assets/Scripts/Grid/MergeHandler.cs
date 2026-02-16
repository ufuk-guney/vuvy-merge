using System.Linq;
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
        if (!dragData.HasValue) return false;

        var chainData = _database.ItemChainDataList.FirstOrDefault(c => c.ChainType == dragData.Value.ChainType);
        if (chainData == null) return false;

        int nextLevel = dragData.Value.Level + 1;
        if (nextLevel >= chainData.Sprites.Count)
        {
            Debug.Log($"Max seviye! {chainData.ChainType} için merge yapılamaz.");
            return false;
        }

        var dragView = _gridState.GetViewAt(dragPos);
        var dropView = _gridState.GetViewAt(dropPos);

        _gridState.RemoveItem(dragPos);
        _gridState.RemoveItem(dropPos);

        _itemFactory.ReturnView(dragView);
        _itemFactory.ReturnView(dropView);

        _itemFactory.SpawnItem(chainData.ChainType, nextLevel, dropPos);

        return true;
    }
}

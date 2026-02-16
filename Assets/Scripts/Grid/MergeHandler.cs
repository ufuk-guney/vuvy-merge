using System.Linq;
using UnityEngine;

public class MergeHandler
{
    private readonly GridStateManager _gridState;
    private readonly ObjectPool<GridItem> _itemPool;
    private readonly BoardItemConfig _database;

    public MergeHandler(GridStateManager gridState, ObjectPool<GridItem> itemPool, BoardItemConfig database)
    {
        _gridState = gridState;
        _itemPool = itemPool;
        _database = database;
    }

    public bool TryMerge(IGridItem draggedItem, IGridItem targetItem, Vector2Int dragPos, Vector2Int dropPos)
    {
        var chainData = _database.ItemChainDataList.FirstOrDefault(c => c.ChainType == draggedItem.ChainType);
        if (chainData == null) return false;

        int nextLevel = draggedItem.Level + 1;
        if (nextLevel >= chainData.Sprites.Count)
        {
            Debug.Log($"Max seviye! {chainData.ChainType} için merge yapılamaz.");
            return false;
        }

        _gridState.RemoveItem(dragPos);
        _gridState.RemoveItem(dropPos);

        ReturnToPool(draggedItem);
        ReturnToPool(targetItem);

        var newItem = _itemPool.Get();
        newItem.Setup(chainData.ChainType, chainData.Sprites[nextLevel], nextLevel);
        newItem.Transform.position = new Vector3(dropPos.x, dropPos.y, 0f);
        _gridState.PlaceItem(dropPos, newItem);

        return true;
    }

    private void ReturnToPool(IGridItem item)
    {
        item.ResetItem();
        if (item is GridItem gridItem)
            _itemPool.Return(gridItem);
    }
}

using System;
using UnityEngine;
using VContainer.Unity;

public class ItemSpawner : IInitializable, IDisposable
{
    private readonly ObjectPool<GridItem> _itemPool;
    private readonly GridStateManager _gridState;
    private readonly BoardItemConfig _database;

    public ItemSpawner(ObjectPool<GridItem> itemPool, GridStateManager gridState, BoardItemConfig database)
    {
        _itemPool = itemPool;
        _gridState = gridState;
        _database = database;
    }

    public void Initialize()
    {
        EventManager.Subscribe(EventType.OnGenerateClick, OnGenerate);
    }

    private void OnGenerate()
    {
        if (!_gridState.TryGetEmptyPosition(out var pos))
            return;

        var chainDataList = _database.ItemChainDataList;
        var chainData = chainDataList[UnityEngine.Random.Range(0, chainDataList.Count)];
        var sprite = chainData.Sprites[0];

        IGridItem item = _itemPool.Get();
        item.Setup(chainData.ChainType, sprite, 0);
        item.Transform.position = new Vector3(pos.x, pos.y, 0f);

        _gridState.PlaceItem(pos, item);
    }

    public void Dispose()
    {
        EventManager.Unsubscribe(EventType.OnGenerateClick, OnGenerate);
    }
}

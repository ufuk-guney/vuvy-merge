using System;
using UnityEngine;
using VContainer.Unity;

public class ItemFactory : IInitializable, IDisposable
{
    private readonly ObjectPool<GridItem> _itemPool;
    private readonly GridStateManager _gridState;
    private readonly BoardItemConfig _database;

    public ItemFactory(BoardItemConfig database, GridStateManager gridState, LifetimeScope scope)
    {
        _database = database;
        _gridState = gridState;
        _itemPool = new ObjectPool<GridItem>(database.ItemPrefab, scope.transform, 10);
    }

    public void Initialize()
    {
        _itemPool.Prewarm();
        EventManager.Subscribe(EventType.OnGenerateClick, OnGenerate);
    }

    private void OnGenerate()
    {
        if (!_gridState.TryGetEmptyPosition(out var pos))
            return;

        var chainDataList = _database.ItemChainDataList;
        var chainData = chainDataList[UnityEngine.Random.Range(0, chainDataList.Count)];

        SpawnItem(chainData.ChainType, 0, pos);
    }

    public void SpawnItem(ItemChainType chainType, int level, Vector2Int pos)
    {
        var chainData = _database.ItemChainDataList.Find(c => c.ChainType == chainType);
        if (chainData == null) return;

        var data = new GridItemData(chainType, level);
        var view = _itemPool.Get();
        view.ApplyVisual(chainData.Sprites[level]);
        view.Transform.position = new Vector3(pos.x, pos.y, 0f);

        _gridState.PlaceItem(pos, data, view);
    }

    public void ReturnView(IGridItemView view)
    {
        view.ResetView();
        if (view is GridItem gridItem)
            _itemPool.Return(gridItem);
    }

    public void Dispose()
    {
        EventManager.Unsubscribe(EventType.OnGenerateClick, OnGenerate);
    }
}

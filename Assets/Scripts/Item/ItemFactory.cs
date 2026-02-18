using System;
using UnityEngine;
using VContainer.Unity;

public class ItemFactory : IInitializable, IDisposable, IItemSpawner
{
    private readonly ObjectPool<ItemView> _itemPool;
    private readonly GridStateManager _gridState;
    private readonly BoardItemConfig _database;

    public ItemFactory(BoardItemConfig database, GridStateManager gridState, LifetimeScope scope)
    {
        _database = database;
        _gridState = gridState;
        _itemPool = new ObjectPool<ItemView>(database.ItemPrefab, scope.transform, Constants.Pool.ItemPoolInitialSize);
    }

    public void Initialize()
    {
        _itemPool.Prewarm();
        EventManager.Subscribe(EventType.OnGenerateClick, OnGenerate);
    }

    private void OnGenerate()
    {
        if (!_gridState.TryGetEmptyPosition(out var pos))
        {
            EventManager.Trigger<string>(EventType.OnWarning, "Grid is full!");
            return;
        }

        var chainDataList = _database.ItemChainDataList;
        var chainData = chainDataList[UnityEngine.Random.Range(0, chainDataList.Count)];

        SpawnItem(chainData.ChainType, Constants.Item.StartingLevel, pos);
    }

    public void SpawnItem(ItemChainType chainType, int level, SlotPosition pos)
    {
        var chainData = _database.ItemChainDataList.Find(c => c.ChainType == chainType);
        if (chainData == null) return;

        var data = new ItemData(chainType, level);
        var view = _itemPool.Get();
        view.Initialize(chainData.Sprites[level]);
        view.Transform.position = pos.ToWorldPosition();

        _gridState.PlaceItem(pos, data, view);
    }

    public void ReturnView(IItemView view)
    {
        view.ResetView();
        if (view is ItemView gridItem)
            _itemPool.Return(gridItem);
    }

    public void Dispose()
    {
        EventManager.Unsubscribe(EventType.OnGenerateClick, OnGenerate);
    }
}

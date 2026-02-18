using System;
using VContainer.Unity;

public class ItemFactory : IInitializable, IItemSpawner, IDisposable
{
    private readonly ObjectPool<ItemView> _itemPool;
    private readonly IGridWriter _gridWriter;
    private readonly BoardItemConfig _database;

    public ItemFactory(BoardItemConfig database, IGridWriter gridWriter, LifetimeScope scope)
    {
        _database = database;
        _gridWriter = gridWriter;
        _itemPool = new ObjectPool<ItemView>((ItemView)database.ItemPrefab, scope.transform, Constants.Pool.ItemPoolInitialSize);
    }

    public void Initialize()
    {
        _itemPool.Prewarm();
        EventBus.Subscribe(EventType.OnGenerateClick, OnGenerate);
    }

    private void OnGenerate()
    {
        if (!_gridWriter.TryGetEmptyPosition(out var pos))
        {
            EventBus.Trigger<string>(EventType.OnWarning, "Grid is full!");
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

        _gridWriter.PlaceItem(pos, data, view);
    }

    public void ReturnView(IItemView view)
    {
        view.ResetView();
        if (view is ItemView gridItem)
            _itemPool.Return(gridItem);
    }

    public void Dispose()
    {
        EventBus.Unsubscribe(EventType.OnGenerateClick, OnGenerate);
    }
}

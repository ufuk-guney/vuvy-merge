using System;
using System.Collections.Generic;
using VContainer.Unity;
using VuvyMerge.Data;

namespace VuvyMerge.Grid
{
    public class ItemFactory : IInitializable, IItemSpawner, IDisposable
    {
        private readonly ObjectPool<ItemView> _itemPool;
        private readonly IGridWriter _gridWriter;
        private readonly List<ItemChainData> _chainDataList;
        private readonly Dictionary<ItemChainType, ItemChainData> _chainDataByType;

        public ItemFactory(BoardItemConfig database, IGridWriter gridWriter, LifetimeScope scope)
        {
            _gridWriter = gridWriter;
            _chainDataList = database.ItemChainDataList;
            _chainDataByType = database.GetChainLookUp();
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
                EventBus.Trigger<string>(EventType.OnWarning, Constants.Text.GridFullWarning);
                return;
            }

            var chainData = _chainDataList[UnityEngine.Random.Range(0, _chainDataList.Count)];
            SpawnItem(chainData.ChainType, Constants.Item.StartingLevel, pos);
        }

        public void SpawnItem(ItemChainType chainType, int level, SlotPosition pos)
        {
            if (!_chainDataByType.TryGetValue(chainType, out var chainData)) return;

            var view = _itemPool.Get();
            view.Initialize(chainData.Sprites[level]);
            view.Transform.position = pos.ToWorldPosition();

            _gridWriter.PlaceItem(pos, new ItemData(chainType, level), view);
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
}

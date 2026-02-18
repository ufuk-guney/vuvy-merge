using System.Collections.Generic;
using UnityEngine;
using VuvyMerge.Grid;


namespace VuvyMerge.Data
{
    [CreateAssetMenu(fileName = "BoardItemDatabase", menuName = "Game/Board Item Database")]
    public class BoardItemConfig : ScriptableObject
    {
        [SerializeField] private SlotView _slotPrefab;
        [SerializeField] private ItemView _itemPrefab;

        public ISlotView SlotPrefab => _slotPrefab;
        public IItemView ItemPrefab => _itemPrefab;
        public List<ItemChainData> ItemChainDataList;

        private Dictionary<ItemChainType, ItemChainData> _chainLookup;

        public Dictionary<ItemChainType, ItemChainData> GetChainLookUp()//o(1)
        {
            if (_chainLookup is not null) return _chainLookup;

            _chainLookup = new Dictionary<ItemChainType, ItemChainData>(ItemChainDataList.Count);
            foreach (var chain in ItemChainDataList)
                _chainLookup[chain.ChainType] = chain;
            return _chainLookup;
        }
    }
}

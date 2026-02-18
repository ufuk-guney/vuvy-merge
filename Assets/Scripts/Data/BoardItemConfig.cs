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
    }
}

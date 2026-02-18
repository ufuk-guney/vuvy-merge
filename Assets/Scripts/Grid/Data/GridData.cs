using System.Collections.Generic;
using UnityEngine;
using VuvyMerge.Data;

namespace VuvyMerge.Grid
{
    public class GridData
    {
        private SlotData[,] _slots;
        private readonly List<SlotPosition> _emptyPositions = new();//this is for o(n)
        private readonly List<SlotPosition> _mergeablePositions = new();
        private GridSize _size;

        public void Initialize(GridSize gridSize)
        {
            _size = gridSize;
            _slots = new SlotData[_size.Width, _size.Height];

            _emptyPositions.Clear();
            for (int x = 0; x < _size.Width; x++)
                for (int y = 0; y < _size.Height; y++)
                {
                    var pos = new SlotPosition(x, y);
                    _slots[x, y] = new SlotData(pos);
                    _emptyPositions.Add(pos);
                }
        }

        public SlotData GetSlotAt(SlotPosition pos)
        {
            return _slots[pos.X, pos.Y];
        }

        public bool IsValidPosition(SlotPosition pos)
        {
            return pos.X >= 0 && pos.X < _size.Width && pos.Y >= 0 && pos.Y < _size.Height;
        }

        public bool TryGetEmptyPosition(out SlotPosition position)
        {
            if (_emptyPositions.Count == 0)
            {
                position = default;
                return false;
            }

            position = _emptyPositions[Random.Range(0, _emptyPositions.Count)];
            return true;
        }

        public void PlaceItem(SlotPosition pos, ItemData data)
        {
            _slots[pos.X, pos.Y].Place(data);
            _emptyPositions.Remove(pos);
        }

        public void RemoveItem(SlotPosition pos)
        {
            _slots[pos.X, pos.Y].Clear();
            _emptyPositions.Add(pos);
        }

        public void MoveItem(SlotPosition from, SlotPosition to)
        {
            var fromSlot = _slots[from.X, from.Y];
            _slots[to.X, to.Y].Place(fromSlot.Data.Value);
            _slots[from.X, from.Y].Clear();

            _emptyPositions.Add(from);
            int index = _emptyPositions.IndexOf(to);
            if (index >= 0)
            {
                _emptyPositions[index] = _emptyPositions[^1];
                _emptyPositions.RemoveAt(_emptyPositions.Count - 1);
            }
        }

        public List<SlotPosition> GetMergeablePositions(ItemData draggedData, SlotPosition excludePos, BoardItemConfig config)
        {
            _mergeablePositions.Clear();

            var chainData = config.ItemChainDataList.Find(c => c.ChainType == draggedData.ChainType);
            if (chainData == null) return _mergeablePositions;

            for (int x = 0; x < _size.Width; x++)
                for (int y = 0; y < _size.Height; y++)
                {
                    var pos = new SlotPosition(x, y);
                    if (pos == excludePos) continue;

                    var slot = _slots[x, y];
                    if (slot.Data.HasValue && draggedData.CanMerge(slot.Data.Value, chainData.MaxLevel))
                        _mergeablePositions.Add(pos);
                }

            return _mergeablePositions;
        }
    }
}

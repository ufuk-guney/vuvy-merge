using System.Collections.Generic;
using UnityEngine;

namespace VuvyMerge.Grid
{
    public class GridView
    {
        private readonly ISlotView _tilePrefab;
        private ISlotView[,] _slotViews;
        private GridSize _gridSize;

        public GridView(ISlotView tilePrefab)
        {
            _tilePrefab = tilePrefab;
        }

        public void GenerateSlots(GridSize gridSize, Transform parent)
        {
            _gridSize = gridSize;
            _slotViews = new ISlotView[_gridSize.Width, _gridSize.Height];

            for (int y = 0; y < _gridSize.Height; y++)
            {
                for (int x = 0; x < _gridSize.Width; x++)
                {
                    var pos = new SlotPosition(x, y);
                    _slotViews[x, y] = (ISlotView)Object.Instantiate((MonoBehaviour)_tilePrefab, pos.ToWorldPosition(), Quaternion.identity, parent);
                }
            }
        }

        public void RegisterItemView(SlotPosition pos, IItemView view)
        {
            _slotViews[pos.X, pos.Y].SetItemView(view);
        }

        public void UnregisterItemView(SlotPosition pos)
        {
            _slotViews[pos.X, pos.Y].ClearItemView();
        }

        public void MoveItemView(SlotPosition from, SlotPosition to)
        {
            var view = _slotViews[from.X, from.Y].ItemView;
            _slotViews[from.X, from.Y].ClearItemView();
            _slotViews[to.X, to.Y].SetItemView(view);
        }

        public IItemView GetItemViewAt(SlotPosition pos)
        {
            return _slotViews[pos.X, pos.Y].ItemView;
        }

        public void HighlightPositions(IReadOnlyList<SlotPosition> positions)
        {
            foreach (var pos in positions)
                _slotViews[pos.X, pos.Y].SetHighlight(Color.green);
        }

        public void ResetAllHighlights()
        {
            for (int x = 0; x < _gridSize.Width; x++)
                for (int y = 0; y < _gridSize.Height; y++)
                    _slotViews[x, y].ResetHighlight();
        }
    }
}

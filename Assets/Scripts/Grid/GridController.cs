using System.Collections.Generic;
using VContainer.Unity;
using VuvyMerge.Data;

namespace VuvyMerge.Grid
{
    public class GridController : IInitializable, IGridReader, IGridWriter, IGridHighlighter
    {
        private readonly GridData _gridData;
        private readonly GridView _gridView;
        private readonly Dictionary<ItemChainType, ItemChainData> _chainLookup;
        private readonly LifetimeScope _scope;

        public GridController(BoardItemConfig database, LifetimeScope scope)
        {
            _scope = scope;
            _gridData = new GridData();
            _gridView = new GridView(database.SlotPrefab);
            _chainLookup = database.GetChainLookUp();
        }

        public void Initialize()
        {
            var gridSize = new GridSize(Constants.Grid.Width, Constants.Grid.Height);
            _gridView.GenerateSlots(gridSize, _scope.transform);
            _gridData.Initialize(gridSize);
        }

        // IGridReader
        public bool IsValidPosition(SlotPosition pos)    => _gridData.IsValidPosition(pos);
        public SlotData GetSlotAt(SlotPosition pos)      => _gridData.GetSlotAt(pos);
        public IItemView GetItemViewAt(SlotPosition pos) => _gridView.GetItemViewAt(pos);

        // IGridWriter
        public bool TryGetEmptyPosition(out SlotPosition pos) => _gridData.TryGetEmptyPosition(out pos);

        public void PlaceItem(SlotPosition pos, ItemData data, IItemView view)
        {
            _gridData.PlaceItem(pos, data);
            _gridView.RegisterItemView(pos, view);
        }

        public void RemoveItem(SlotPosition pos)
        {
            _gridData.RemoveItem(pos);
            _gridView.UnregisterItemView(pos);
        }

        public void MoveItem(SlotPosition from, SlotPosition to)
        {
            _gridData.MoveItem(from, to);
            _gridView.MoveItemView(from, to);
        }

        // IGridHighlighter
        public void HighlightMergeablePositions(ItemData draggedData, SlotPosition excludePos)
        {
            if (!_chainLookup.TryGetValue(draggedData.ChainType, out var chainData)) return;
            var positions = _gridData.GetMergeablePositions(draggedData, excludePos, chainData.MaxLevel);
            _gridView.HighlightPositions(positions);
        }

        public void ResetHighlights() => _gridView.ResetAllHighlights();
    }
}

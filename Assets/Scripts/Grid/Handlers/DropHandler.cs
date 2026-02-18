namespace VuvyMerge.Grid
{
    public class DropHandler
    {
        private readonly IGridReader _gridReader;
        private readonly IGridWriter _gridWriter;
        private readonly MergeHandler _mergeHandler;

        public DropHandler(IGridReader gridReader, IGridWriter gridWriter, MergeHandler mergeHandler)
        {
            _gridReader = gridReader;
            _gridWriter = gridWriter;
            _mergeHandler = mergeHandler;
        }

        public void HandleDrop(IItemView draggedView, SlotPosition startPos, SlotPosition dropPos)
        {
            if (!_gridReader.IsValidPosition(dropPos))
            {
                draggedView.AnimateToPosition(startPos);
                return;
            }

            if (dropPos == startPos)
            {
                draggedView.SnapToPosition(startPos);
                return;
            }

            var dropSlot = _gridReader.GetSlotAt(dropPos);
            if (dropSlot.IsEmpty)
            {
                _gridWriter.MoveItem(startPos, dropPos);
                draggedView.SnapToPosition(dropPos);
                return;
            }

            HandleOccupiedDrop(draggedView, startPos, dropPos);
        }

        private void HandleOccupiedDrop(IItemView draggedView, SlotPosition startPos, SlotPosition dropPos)
        {
            if (_mergeHandler.TryMerge(startPos, dropPos)) return;
            draggedView.AnimateToPosition(startPos);
        }
    }
}

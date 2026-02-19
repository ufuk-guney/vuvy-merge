namespace VuvyMerge.Grid
{
    public class DropService
    {
        private readonly IGridReader _gridReader;
        private readonly IGridWriter _gridWriter;
        private readonly MergeService _mergeService;

        public DropService(IGridReader gridReader, IGridWriter gridWriter, MergeService mergeService)
        {
            _gridReader = gridReader;
            _gridWriter = gridWriter;
            _mergeService = mergeService;
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
            if (_mergeService.TryMerge(startPos, dropPos)) return;
            draggedView.AnimateToPosition(startPos);
        }
    }
}

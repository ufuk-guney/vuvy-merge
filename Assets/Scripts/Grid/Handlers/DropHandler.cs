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

        TryMergeOrReturn(draggedView, startPos, dropPos);
    }

    private void TryMergeOrReturn(IItemView draggedView, SlotPosition startPos, SlotPosition dropPos)
    {
        var dragSlot = _gridReader.GetSlotAt(startPos);
        var dropSlot = _gridReader.GetSlotAt(dropPos);

        if (dragSlot.Data.HasValue && dropSlot.Data.HasValue
            && dragSlot.Data.Value.CanMerge(dropSlot.Data.Value)
            && _mergeHandler.TryMerge(startPos, dropPos))
        {
            return;
        }

        draggedView.AnimateToPosition(startPos);
    }
}

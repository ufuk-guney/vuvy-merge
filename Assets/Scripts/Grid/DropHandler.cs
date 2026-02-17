public class DropHandler
{
    private readonly GridStateManager _gridState;
    private readonly MergeHandler _mergeHandler;

    public DropHandler(GridStateManager gridState, MergeHandler mergeHandler)
    {
        _gridState = gridState;
        _mergeHandler = mergeHandler;
    }

    public void HandleDrop(IItemView draggedView, SlotPosition startPos, SlotPosition dropPos)
    {
        if (!_gridState.IsValidPosition(dropPos))
        {
            draggedView.AnimateToPosition(startPos);
            return;
        }

        if (dropPos == startPos)
        {
            draggedView.SnapToPosition(startPos);
            return;
        }

        var dropSlot = _gridState.GetSlotAt(dropPos);
        if (dropSlot.IsEmpty)
        {
            _gridState.MoveItem(startPos, dropPos);
            draggedView.SnapToPosition(dropPos);
            return;
        }

        TryMergeOrReturn(draggedView, startPos, dropPos);
    }

    private void TryMergeOrReturn(IItemView draggedView, SlotPosition startPos, SlotPosition dropPos)
    {
        var dragSlot = _gridState.GetSlotAt(startPos);
        var dropSlot = _gridState.GetSlotAt(dropPos);

        if (dragSlot.Data.HasValue && dropSlot.Data.HasValue
            && dragSlot.Data.Value.CanMerge(dropSlot.Data.Value)
            && _mergeHandler.TryMerge(startPos, dropPos))
        {
            return;
        }

        draggedView.AnimateToPosition(startPos);
    }
}

using UnityEngine;

public class DropHandler
{
    private readonly GridStateManager _gridState;
    private readonly MergeHandler _mergeHandler;

    public DropHandler(GridStateManager gridState, MergeHandler mergeHandler)
    {
        _gridState = gridState;
        _mergeHandler = mergeHandler;
    }

    public void HandleDrop(IGridItem draggedItem, Vector2Int startGridPos, Vector2Int dropGridPos)
    {
        if (!_gridState.IsValidPosition(dropGridPos))
        {
            draggedItem.AnimateToPosition(startGridPos);
            return;
        }

        if (dropGridPos == startGridPos)
        {
            draggedItem.SnapToPosition(startGridPos);
            return;
        }

        if (_gridState.IsEmpty(dropGridPos))
        {
            _gridState.MoveItem(startGridPos, dropGridPos);
            draggedItem.SnapToPosition(dropGridPos);
            return;
        }

        TryMergeOrReturn(draggedItem, startGridPos, dropGridPos);
    }

    private void TryMergeOrReturn(IGridItem draggedItem, Vector2Int startGridPos, Vector2Int dropGridPos)
    {
        var targetItem = _gridState.GetItemAt(dropGridPos);

        if (draggedItem.CanMerge(targetItem)
            && _mergeHandler.TryMerge(draggedItem, targetItem, startGridPos, dropGridPos))
        {
            return;
        }

        draggedItem.AnimateToPosition(startGridPos);
    }
}

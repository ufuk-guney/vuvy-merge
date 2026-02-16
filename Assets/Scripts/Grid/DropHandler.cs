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

    public void HandleDrop(IGridItemView draggedView, Vector2Int startGridPos, Vector2Int dropGridPos)
    {
        if (!_gridState.IsValidPosition(dropGridPos))
        {
            draggedView.AnimateToPosition(startGridPos);
            return;
        }

        if (dropGridPos == startGridPos)
        {
            draggedView.SnapToPosition(startGridPos);
            return;
        }

        if (_gridState.IsEmpty(dropGridPos))
        {
            _gridState.MoveItem(startGridPos, dropGridPos);
            draggedView.SnapToPosition(dropGridPos);
            return;
        }

        TryMergeOrReturn(draggedView, startGridPos, dropGridPos);
    }

    private void TryMergeOrReturn(IGridItemView draggedView, Vector2Int startGridPos, Vector2Int dropGridPos)
    {
        var dragData = _gridState.GetDataAt(startGridPos);
        var dropData = _gridState.GetDataAt(dropGridPos);

        if (dragData.HasValue && dropData.HasValue
            && dragData.Value.CanMerge(dropData.Value)
            && _mergeHandler.TryMerge(startGridPos, dropGridPos))
        {
            return;
        }

        draggedView.AnimateToPosition(startGridPos);
    }
}

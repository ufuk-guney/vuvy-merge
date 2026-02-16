using DG.Tweening;
using UnityEngine;

public class DragHandler
{
    private const float DragScale = 1.1f;
    private const float DragScaleDuration = 0.2f;
    private const int DragSortingOrder = 10;

    private readonly GridStateManager _gridState;
    private readonly DropHandler _dropHandler;

    private IGridItem _draggedItem;
    private Vector2Int _startGridPos;
    private bool _isDragging;
    private int _originalSortingOrder;

    public DragHandler(GridStateManager gridState, DropHandler dropHandler)
    {
        _gridState = gridState;
        _dropHandler = dropHandler;
    }

    public bool TryStartDrag(Vector2Int gridPos)
    {
        if (!_gridState.IsValidPosition(gridPos)) return false;
        if (_gridState.IsEmpty(gridPos)) return false;

        _draggedItem = _gridState.GetItemAt(gridPos);
        _startGridPos = gridPos;
        _isDragging = true;

        _draggedItem.Transform.DOScale(DragScale, DragScaleDuration).SetEase(Ease.OutBack);
        _originalSortingOrder = _draggedItem.SpriteRenderer.sortingOrder;
        _draggedItem.SpriteRenderer.sortingOrder = DragSortingOrder;

        return true;
    }

    public void UpdateDragPosition(Vector3 worldPos)
    {
        if (!_isDragging || _draggedItem == null) return;
        _draggedItem.Transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
    }

    public void EndDrag(Vector2Int dropGridPos)
    {
        if (!_isDragging || _draggedItem == null) return;
        _isDragging = false;

        _draggedItem.Transform.DOScale(1f, DragScaleDuration).SetEase(Ease.OutBack);
        _draggedItem.SpriteRenderer.sortingOrder = _originalSortingOrder;

        _dropHandler.HandleDrop(_draggedItem, _startGridPos, dropGridPos);
        _draggedItem = null;
    }
}

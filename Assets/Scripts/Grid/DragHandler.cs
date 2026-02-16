using DG.Tweening;
using UnityEngine;

public class DragHandler
{
    private const float DragScale = 1.1f;
    private const float DragScaleDuration = 0.2f;
    private const int DragSortingOrder = 10;

    private readonly GridStateManager _gridState;
    private readonly DropHandler _dropHandler;

    private IGridItemView _draggedView;
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

        _draggedView = _gridState.GetViewAt(gridPos);
        _startGridPos = gridPos;
        _isDragging = true;

        _draggedView.Transform.DOScale(DragScale, DragScaleDuration).SetEase(Ease.OutBack);
        _originalSortingOrder = _draggedView.SpriteRenderer.sortingOrder;
        _draggedView.SpriteRenderer.sortingOrder = DragSortingOrder;

        return true;
    }

    public void UpdateDragPosition(Vector3 worldPos)
    {
        if (!_isDragging || _draggedView == null) return;
        _draggedView.Transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
    }

    public void EndDrag(Vector2Int dropGridPos)
    {
        if (!_isDragging || _draggedView == null) return;
        _isDragging = false;

        _draggedView.Transform.DOScale(1f, DragScaleDuration).SetEase(Ease.OutBack);
        _draggedView.SpriteRenderer.sortingOrder = _originalSortingOrder;

        _dropHandler.HandleDrop(_draggedView, _startGridPos, dropGridPos);
        _draggedView = null;
    }
}

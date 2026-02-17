using DG.Tweening;
using UnityEngine;

public class DragHandler
{
    private const float DragScale = 1.1f;
    private const float DragScaleDuration = 0.2f;
    private const int DragSortingOrder = 10;

    private readonly GridStateManager _gridState;
    private readonly DropHandler _dropHandler;
    private readonly TileHandler _tileHandler;

    private IItemView _draggedView;
    private SlotPosition _startPos;
    private bool _isDragging;
    private int _originalSortingOrder;

    public DragHandler(GridStateManager gridState, DropHandler dropHandler, TileHandler tileHandler)
    {
        _gridState = gridState;
        _dropHandler = dropHandler;
        _tileHandler = tileHandler;
    }

    public bool TryStartDrag(SlotPosition pos)
    {
        if (!_gridState.IsValidPosition(pos)) return false;

        var slot = _gridState.GetSlotAt(pos);
        if (slot.IsEmpty) return false;

        _draggedView = slot.View;
        _startPos = pos;
        _isDragging = true;

        _draggedView.Transform.DOScale(DragScale, DragScaleDuration).SetEase(Ease.OutBack);
        _originalSortingOrder = _draggedView.SpriteRenderer.sortingOrder;
        _draggedView.SpriteRenderer.sortingOrder = DragSortingOrder;

        if (slot.Data.HasValue)
        {
            _tileHandler.HighlightMergeableTiles(slot.Data.Value, _gridState, pos);
        }

        return true;
    }

    public void UpdateDragPosition(Vector3 worldPos)
    {
        if (!_isDragging || _draggedView == null) return;
        _draggedView.Transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
    }

    public void EndDrag(SlotPosition dropPos)
    {
        if (!_isDragging || _draggedView == null) return;
        _isDragging = false;

        _tileHandler.ResetAllHighlights();

        _draggedView.Transform.DOScale(1f, DragScaleDuration).SetEase(Ease.OutBack);
        _draggedView.SpriteRenderer.sortingOrder = _originalSortingOrder;

        _dropHandler.HandleDrop(_draggedView, _startPos, dropPos);
        _draggedView = null;
    }
}

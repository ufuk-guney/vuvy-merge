using DG.Tweening;
using UnityEngine;

public class DragHandler
{
    private readonly IGridReader _gridReader;
    private readonly IGridHighlighter _gridHighlighter;
    private readonly DropHandler _dropHandler;

    private IItemView _draggedView;
    private SlotPosition _startPos;
    private bool _isDragging;
    private int _originalSortingOrder;

    public DragHandler(IGridReader gridReader, IGridHighlighter gridHighlighter, DropHandler dropHandler)
    {
        _gridReader = gridReader;
        _gridHighlighter = gridHighlighter;
        _dropHandler = dropHandler;
    }

    public bool TryStartDrag(SlotPosition pos)
    {
        if (!_gridReader.IsValidPosition(pos)) return false;

        var slot = _gridReader.GetSlotAt(pos);
        if (slot.IsEmpty) return false;

        _draggedView = _gridReader.GetItemViewAt(pos);
        _startPos = pos;
        _isDragging = true;

        _draggedView.Transform.DOScale(Constants.Animation.DragScale, Constants.Animation.TweenDuration).SetEase(Ease.OutBack);
        _originalSortingOrder = _draggedView.SortingOrder;
        _draggedView.SortingOrder = Constants.Animation.DragSortingOrder;

        if (slot.Data.HasValue)
            _gridHighlighter.HighlightMergeablePositions(slot.Data.Value, pos);

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

        _gridHighlighter.ResetHighlights();

        _draggedView.Transform.DOScale(1f, Constants.Animation.TweenDuration).SetEase(Ease.OutBack);
        _draggedView.SortingOrder = _originalSortingOrder;

        _dropHandler.HandleDrop(_draggedView, _startPos, dropPos);
        _draggedView = null;
    }
}

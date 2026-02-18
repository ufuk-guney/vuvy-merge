using UnityEngine;

public class DragHandler
{
    private readonly IGridReader _gridReader;
    private readonly IGridHighlighter _gridHighlighter;
    private readonly DropHandler _dropHandler;

    private DragSession? _activeSession;

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

        var view = _gridReader.GetItemViewAt(pos);
        _activeSession = new DragSession(view, pos, view.SortingOrder);
        view.BeginDrag();

        if (slot.Data.HasValue)
            _gridHighlighter.HighlightMergeablePositions(slot.Data.Value, pos);

        return true;
    }

    public void UpdateDragPosition(Vector3 worldPos)
    {
        if (!_activeSession.HasValue) return;
        _activeSession.Value.View.Transform.position = worldPos;
    }

    public void EndDrag(SlotPosition dropPos)
    {
        if (!_activeSession.HasValue) return;

        var session = _activeSession.Value;
        _activeSession = null;

        _gridHighlighter.ResetHighlights();
        session.View.EndDrag(session.OriginalSortingOrder);
        _dropHandler.HandleDrop(session.View, session.StartPos, dropPos);
    }

    private readonly struct DragSession
    {
        public readonly IItemView View;
        public readonly SlotPosition StartPos;
        public readonly int OriginalSortingOrder;

        public DragSession(IItemView view, SlotPosition startPos, int originalSortingOrder)
        {
            View = view;
            StartPos = startPos;
            OriginalSortingOrder = originalSortingOrder;
        }
    }
}

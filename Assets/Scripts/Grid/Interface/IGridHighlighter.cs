public interface IGridHighlighter
{
    void HighlightMergeablePositions(ItemData draggedData, SlotPosition excludePos);
    void ResetHighlights();
}

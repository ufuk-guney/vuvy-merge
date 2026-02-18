namespace VuvyMerge.Grid
{
    public interface IGridHighlighter
    {
        void HighlightMergeablePositions(ItemData draggedData, SlotPosition excludePos);
        void ResetHighlights();
    }
}

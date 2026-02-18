public interface IGridWriter
{
    bool TryGetEmptyPosition(out SlotPosition pos);
    void PlaceItem(SlotPosition pos, ItemData data, IItemView view);
    void RemoveItem(SlotPosition pos);
    void MoveItem(SlotPosition from, SlotPosition to);
}

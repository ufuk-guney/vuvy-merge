public interface IGridReader
{
    bool IsValidPosition(SlotPosition pos);
    SlotData GetSlotAt(SlotPosition pos);
    IItemView GetItemViewAt(SlotPosition pos);
}

public struct Slot
{
    public SlotPosition Position { get; }
    public ItemData? Data { get; internal set; }
    public IItemView View { get; internal set; }
    public bool IsEmpty => !Data.HasValue;

    public Slot(SlotPosition position)
    {
        Position = position;
        Data = null;
        View = null;
    }

    public void Place(ItemData data, IItemView view)
    {
        Data = data;
        View = view;
    }

    public void Clear()
    {
        Data = null;
        View = null;
    }
}

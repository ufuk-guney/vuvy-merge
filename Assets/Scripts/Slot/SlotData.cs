namespace VuvyMerge.Grid
{
    public struct SlotData
    {
        public SlotPosition Position { get; }
        public ItemData? Data { get; internal set; }
        public bool IsEmpty => !Data.HasValue;

        public SlotData(SlotPosition position)
        {
            Position = position;
            Data = null;
        }

        public void Place(ItemData data)
        {
            Data = data;
        }

        public void Clear()
        {
            Data = null;
        }
    }
}

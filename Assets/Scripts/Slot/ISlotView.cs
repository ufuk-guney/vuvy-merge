namespace VuvyMerge.Grid
{
    public interface ISlotView : IHighlightable
    {
        IItemView ItemView { get; }
        void SetItemView(IItemView view);
        void ClearItemView();
    }
}

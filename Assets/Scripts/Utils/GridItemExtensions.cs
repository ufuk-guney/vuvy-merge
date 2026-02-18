using DG.Tweening;

public static class GridItemExtensions
{
    public static void BeginDrag(this IItemView view)
    {
        view.Transform.DOScale(Constants.Animation.DragScale, Constants.Animation.TweenDuration).SetEase(Ease.OutBack);
        view.SortingOrder = Constants.Animation.DragSortingOrder;
    }

    public static void EndDrag(this IItemView view, int originalSortingOrder)
    {
        view.Transform.DOScale(1f, Constants.Animation.TweenDuration).SetEase(Ease.OutBack);
        view.SortingOrder = originalSortingOrder;
    }

    public static void AnimateToPosition(this IItemView view, SlotPosition pos)
    {
        view.Transform.DOScale(Constants.Animation.SquishScale, Constants.Animation.SquishDuration).SetEase(Ease.OutBack);
        view.Transform.DOMove(pos.ToWorldPosition(), Constants.Animation.TweenDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() => view.Transform.DOScale(1f, Constants.Animation.TweenDuration).SetEase(Ease.InSine));
    }

    public static void SnapToPosition(this IItemView view, SlotPosition pos)
    {
        view.Transform.position = pos.ToWorldPosition();
        view.PlayPopScale();
    }

    private static void PlayPopScale(this IItemView view)
    {
        view.Transform.DOScale(Constants.Animation.SquishScale, Constants.Animation.SquishDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() => view.Transform.DOScale(1f, Constants.Animation.TweenDuration).SetEase(Ease.InSine));
    }

    public static bool CanMerge(this ItemData a, ItemData b)
    {
        return a.ChainType == b.ChainType && a.Level == b.Level;
    }

    public static bool CanMerge(this ItemData a, ItemData b, int maxLevel)
    {
        if (!a.CanMerge(b)) return false;
        return (a.Level + 1) <= maxLevel;
    }
}

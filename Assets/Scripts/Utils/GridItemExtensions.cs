using System.Linq;
using DG.Tweening;

public static class GridItemExtensions
{
    private const float TweenDuration = 0.2f;

    public static void AnimateToPosition(this IItemView view, SlotPosition pos)
    {
        view.Transform.DOMove(pos.ToWorldPosition(), TweenDuration).SetEase(Ease.OutBack);
    }

    public static void SnapToPosition(this IItemView view, SlotPosition pos)
    {
        view.Transform.position = pos.ToWorldPosition();
    }

    public static bool CanMerge(this ItemData a, ItemData b)
    {
        return a.ChainType == b.ChainType && a.Level == b.Level;
    }

    public static bool CanMerge(this ItemData a, ItemData b, BoardItemConfig config)
    {
        if (!a.CanMerge(b)) return false;

        var chainData = config.ItemChainDataList.FirstOrDefault(c => c.ChainType == a.ChainType);
        if (chainData == null) return false;

        int nextLevel = a.Level + 1;
        return nextLevel < chainData.Sprites.Count;
    }
}

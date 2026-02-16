using DG.Tweening;
using UnityEngine;

public static class GridItemExtensions
{
    private const float TweenDuration = 0.2f;

    public static void AnimateToPosition(this IGridItem item, Vector2Int gridPos)
    {
        var targetPos = new Vector3(gridPos.x, gridPos.y, 0f);
        item.Transform.DOMove(targetPos, TweenDuration).SetEase(Ease.OutBack);
    }

    public static void SnapToPosition(this IGridItem item, Vector2Int gridPos)
    {
        item.Transform.position = new Vector3(gridPos.x, gridPos.y, 0f);
    }

    public static bool CanMerge(this IGridItem a, IGridItem b)
    {
        return a.ChainType == b.ChainType && a.Level == b.Level;
    }
}

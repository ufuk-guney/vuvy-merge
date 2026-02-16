using DG.Tweening;
using UnityEngine;

public static class GridItemExtensions
{
    private const float TweenDuration = 0.2f;

    public static void AnimateToPosition(this IGridItemView view, Vector2Int gridPos)
    {
        var targetPos = new Vector3(gridPos.x, gridPos.y, 0f);
        view.Transform.DOMove(targetPos, TweenDuration).SetEase(Ease.OutBack);
    }

    public static void SnapToPosition(this IGridItemView view, Vector2Int gridPos)
    {
        view.Transform.position = new Vector3(gridPos.x, gridPos.y, 0f);
    }

    public static bool CanMerge(this GridItemData a, GridItemData b)
    {
        return a.ChainType == b.ChainType && a.Level == b.Level;
    }
}

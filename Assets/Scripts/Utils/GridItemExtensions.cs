using System.Linq;
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

    public static bool CanMerge(this GridItemData a, GridItemData b, BoardItemConfig config)
    {
        if (!a.CanMerge(b)) return false;

        var chainData = config.ItemChainDataList.FirstOrDefault(c => c.ChainType == a.ChainType);
        if (chainData == null) return false;

        int nextLevel = a.Level + 1;
        return nextLevel < chainData.Sprites.Count;
    }
}

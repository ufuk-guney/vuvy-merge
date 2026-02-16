using UnityEngine;

public interface IGridItem
{
    ItemChainType ChainType { get; }
    int Level { get; }
    Transform Transform { get; }
    SpriteRenderer SpriteRenderer { get; }
    void Setup(ItemChainType chainType, Sprite sprite, int level);
    void ResetItem();
}

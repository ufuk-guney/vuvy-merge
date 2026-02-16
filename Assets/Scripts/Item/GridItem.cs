using UnityEngine;

public class GridItem : MonoBehaviour, IGridItem
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public ItemChainType ChainType { get; private set; }
    public int Level { get; private set; }
    public Transform Transform => transform;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public void Setup(ItemChainType chainType, Sprite sprite, int level)
    {
        ChainType = chainType;
        Level = level;
        _spriteRenderer.sprite = sprite;
    }

    public void ResetItem()
    {
        _spriteRenderer.sprite = null;
        ChainType = default;
        Level = 0;
    }
}

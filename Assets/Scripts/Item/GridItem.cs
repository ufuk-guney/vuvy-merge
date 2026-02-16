using UnityEngine;

public class GridItem : MonoBehaviour, IGridItemView
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public Transform Transform => transform;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public void ApplyVisual(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void ResetView()
    {
        _spriteRenderer.sprite = null;
    }
}

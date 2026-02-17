using DG.Tweening;
using UnityEngine;

public class ItemView : MonoBehaviour, IItemView
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public Transform Transform => transform;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public void Initialize(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    public void ResetView()
    {
        _spriteRenderer.sprite = null;
        transform.localScale = Vector3.one;
        DOTween.Kill(transform);
    }
}

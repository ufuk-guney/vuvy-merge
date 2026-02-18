using DG.Tweening;
using UnityEngine;

namespace VuvyMerge.Grid
{
    public class ItemView : MonoBehaviour, IItemView
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Transform Transform => transform;
        public int SortingOrder
        {
            get => _spriteRenderer.sortingOrder;
            set => _spriteRenderer.sortingOrder = value;
        }

        public void Initialize(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, Constants.Animation.SpawnScaleDuration).SetEase(Ease.OutBack);
        }

        public void ResetView()
        {
            _spriteRenderer.sprite = null;
            transform.localScale = Vector3.one;
            DOTween.Kill(transform);
        }
    }
}

using UnityEngine;

public class SlotView : MonoBehaviour, ISlotView
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    public IItemView ItemView { get; private set; }

    private void Awake()
    {
        _originalColor = _spriteRenderer.color;
    }

    public void SetHighlight(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void ResetHighlight()
    {
        _spriteRenderer.color = _originalColor;
    }

    public void SetItemView(IItemView view)
    {
        ItemView = view;
    }

    public void ClearItemView()
    {
        ItemView = null;
    }
}

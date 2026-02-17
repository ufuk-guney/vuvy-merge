using UnityEngine;

public class TileView : MonoBehaviour, IHighlightable
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

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
}

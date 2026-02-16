using UnityEngine;

public interface IGridItemView
{
    Transform Transform { get; }
    SpriteRenderer SpriteRenderer { get; }
    void ApplyVisual(Sprite sprite);
    void ResetView();
}

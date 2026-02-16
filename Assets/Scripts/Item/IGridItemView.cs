using UnityEngine;

public interface IGridItemView
{
    Transform Transform { get; }
    SpriteRenderer SpriteRenderer { get; }
    void Initialize(Sprite sprite);
    void ResetView();
}

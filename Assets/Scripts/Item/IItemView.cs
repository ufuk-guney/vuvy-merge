using UnityEngine;

public interface IItemView
{
    Transform Transform { get; }
    SpriteRenderer SpriteRenderer { get; }
    void Initialize(Sprite sprite);
    void ResetView();
}

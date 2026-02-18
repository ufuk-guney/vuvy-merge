using UnityEngine;

public interface IItemView
{
    Transform Transform { get; }
    int SortingOrder { get; set; }
    void Initialize(Sprite sprite);
    void ResetView();
}

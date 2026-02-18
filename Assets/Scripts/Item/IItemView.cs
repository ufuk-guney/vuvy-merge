using UnityEngine;

namespace VuvyMerge.Grid
{
    public interface IItemView
    {
        Transform Transform { get; }
        int SortingOrder { get; set; }
        void Initialize(Sprite sprite);
        void ResetView();
    }
}

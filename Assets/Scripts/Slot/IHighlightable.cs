using UnityEngine;

namespace VuvyMerge.Grid
{
    public interface IHighlightable
    {
        void SetHighlight(Color color);
        void ResetHighlight();
    }
}

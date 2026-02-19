using UnityEngine;

namespace VuvyMerge.Grid
{
    public interface IInputHandler
    {
        bool TryStartDrag(SlotPosition pos);
        void UpdateDragPosition(Vector3 worldPos);
        void EndDrag(SlotPosition dropPos);
    }
}

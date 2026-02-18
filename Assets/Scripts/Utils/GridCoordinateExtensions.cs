using UnityEngine;

namespace VuvyMerge.Grid
{
    public static class GridCoordinateExtensions
    {
        public static SlotPosition WorldToGrid(this Vector3 worldPos)
        {
            return new SlotPosition(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));
        }

        public static Vector3? ScreenToWorld(this Vector2 screenPos, Camera camera)
        {
            if (!float.IsFinite(screenPos.x) || !float.IsFinite(screenPos.y)) return null;
            var worldPos = camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, camera.nearClipPlane));
            worldPos.z = 0f;
            return worldPos;
        }

        public static SlotPosition? ScreenToGrid(this Vector2 screenPos, Camera camera)
        {
            var worldPos = screenPos.ScreenToWorld(camera);
            return worldPos?.WorldToGrid();
        }
    }
}

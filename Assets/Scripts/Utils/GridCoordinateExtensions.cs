using UnityEngine;

public static class GridCoordinateExtensions
{
    public static Vector2Int WorldToGrid(this Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));
    }

    public static Vector3 ScreenToWorld(this Vector2 screenPos, Camera camera)
    {
        var worldPos = camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, camera.nearClipPlane));
        worldPos.z = 0f;
        return worldPos;
    }

    public static Vector2Int ScreenToGrid(this Vector2 screenPos, Camera camera)
    {
        return screenPos.ScreenToWorld(camera).WorldToGrid();
    }
}

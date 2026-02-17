using UnityEngine;

public class TileHandler
{
    private readonly BoardItemConfig _database;
    private TileView[,] _tileViews;
    private int _width;
    private int _height;

    public TileHandler(BoardItemConfig database)
    {
        _database = database;
    }

    public void GenerateTiles(GridData gridData, Transform parent)
    {
        _width = gridData.Width;
        _height = gridData.Height;
        _tileViews = new TileView[_width, _height];

        var tilePrefab = _database.TilePrefab;
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                var position = new Vector3(x, y, 0f);
                var tileView = Object.Instantiate(tilePrefab, position, Quaternion.identity, parent);
                _tileViews[x, y] = tileView;
            }
        }
    }

    public void HighlightMergeableTiles(GridItemData draggedData, GridStateManager gridState, Vector2Int excludePos)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (x == excludePos.x && y == excludePos.y) continue;

                var pos = new Vector2Int(x, y);
                var data = gridState.GetDataAt(pos);
                if (data.HasValue && draggedData.CanMerge(data.Value, _database))
                {
                    _tileViews[x, y].SetHighlight(Color.greenYellow);
                }
            }
        }
    }

    public void ResetAllHighlights()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _tileViews[x, y].ResetHighlight();
            }
        }
    }
}

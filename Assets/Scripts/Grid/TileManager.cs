using UnityEngine;

public class TileManager
{
    private readonly BoardItemConfig _database;
    private TileView[,] _tileViews;
    private int _width;
    private int _height;

    public TileManager(BoardItemConfig database)
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
                var pos = new SlotPosition(x, y);
                var tileView = Object.Instantiate(tilePrefab, pos.ToWorldPosition(), Quaternion.identity, parent);
                _tileViews[x, y] = tileView;
            }
        }
    }

    public void HighlightMergeableTiles(ItemData draggedData, GridStateManager gridState, SlotPosition excludePos)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (x == excludePos.X && y == excludePos.Y) continue;

                var slot = gridState.GetSlotAt(new SlotPosition(x, y));
                if (slot.Data.HasValue && draggedData.CanMerge(slot.Data.Value, _database))
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

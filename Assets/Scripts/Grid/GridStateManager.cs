using System.Collections.Generic;
using UnityEngine;

public class GridStateManager
{
    private GridItemData?[,] _dataGrid;
    private IGridItemView[,] _viewGrid;
    private readonly List<Vector2Int> _emptyPositions = new();
    private int _width;
    private int _height;

    public void Initialize(GridData gridData)
    {
        _width = gridData.Width;
        _height = gridData.Height;
        _dataGrid = new GridItemData?[_width, _height];
        _viewGrid = new IGridItemView[_width, _height];

        _emptyPositions.Clear();
        for (int x = 0; x < _width; x++)
            for (int y = 0; y < _height; y++)
                _emptyPositions.Add(new Vector2Int(x, y));
    }

    public bool TryGetEmptyPosition(out Vector2Int position)
    {
        if (_emptyPositions.Count == 0)
        {
            position = default;
            return false;
        }

        position = _emptyPositions[Random.Range(0, _emptyPositions.Count)];
        return true;
    }

    public void PlaceItem(Vector2Int pos, GridItemData data, IGridItemView view)
    {
        _dataGrid[pos.x, pos.y] = data;
        _viewGrid[pos.x, pos.y] = view;
        _emptyPositions.Remove(pos);
    }

    public void RemoveItem(Vector2Int pos)
    {
        _dataGrid[pos.x, pos.y] = null;
        _viewGrid[pos.x, pos.y] = null;
        _emptyPositions.Add(pos);
    }

    public GridItemData? GetDataAt(Vector2Int pos)
    {
        return _dataGrid[pos.x, pos.y];
    }

    public IGridItemView GetViewAt(Vector2Int pos)
    {
        return _viewGrid[pos.x, pos.y];
    }

    public bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < _width && pos.y >= 0 && pos.y < _height;
    }

    public bool IsEmpty(Vector2Int pos)
    {
        return !_dataGrid[pos.x, pos.y].HasValue;
    }

    public void MoveItem(Vector2Int from, Vector2Int to)
    {
        _dataGrid[to.x, to.y] = _dataGrid[from.x, from.y];
        _viewGrid[to.x, to.y] = _viewGrid[from.x, from.y];
        _dataGrid[from.x, from.y] = null;
        _viewGrid[from.x, from.y] = null;

        _emptyPositions.Add(from);
        int index = _emptyPositions.IndexOf(to);
        if (index >= 0)
        {
            _emptyPositions[index] = _emptyPositions[^1];
            _emptyPositions.RemoveAt(_emptyPositions.Count - 1);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class GridStateManager
{
    private IGridItem[,] _grid;
    private readonly List<Vector2Int> _emptyPositions = new();
    private int _width;
    private int _height;

    public void Initialize(GridData gridData)
    {
        _width = gridData.Width;
        _height = gridData.Height;
        _grid = new IGridItem[_width, _height];

        _emptyPositions.Clear();
        for (int x = 0; x < _width; x++)
            for (int y = 0; y < _height; y++)
                _emptyPositions.Add(new Vector2Int(x, y));
    }

    //grid üzerinde boş slot aransa 0(n2), emptyPositions o(n)
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

    public void PlaceItem(Vector2Int pos, IGridItem item)
    {
        _grid[pos.x, pos.y] = item;
        _emptyPositions.Remove(pos);
    }

    public void RemoveItem(Vector2Int pos)
    {
        _grid[pos.x, pos.y] = null;
        _emptyPositions.Add(pos);
    }

    public IGridItem GetItemAt(Vector2Int pos)
    {
        return _grid[pos.x, pos.y];
    }

    public bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < _width && pos.y >= 0 && pos.y < _height;
    }

    public bool IsEmpty(Vector2Int pos)
    {
        return _grid[pos.x, pos.y] == null;
    }

    public void MoveItem(Vector2Int from, Vector2Int to)
    {
        var item = _grid[from.x, from.y];
        _grid[from.x, from.y] = null;
        _grid[to.x, to.y] = item;

        _emptyPositions.Add(from);
        int index = _emptyPositions.IndexOf(to);
        if (index >= 0)
        {
            _emptyPositions[index] = _emptyPositions[^1];
            _emptyPositions.RemoveAt(_emptyPositions.Count - 1);
        }
    }
}

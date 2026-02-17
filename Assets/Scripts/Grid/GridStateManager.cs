using System.Collections.Generic;
using UnityEngine;

public class GridStateManager
{
    private Slot[,] _slots;
    private readonly List<SlotPosition> _emptyPositions = new();//this is for o(n)
    private int _width;
    private int _height;

    public void Initialize(GridData gridData)
    {
        _width = gridData.Width;
        _height = gridData.Height;
        _slots = new Slot[_width, _height];

        _emptyPositions.Clear();
        for (int x = 0; x < _width; x++)
            for (int y = 0; y < _height; y++)
            {
                var pos = new SlotPosition(x, y);
                _slots[x, y] = new Slot(pos);
                _emptyPositions.Add(pos);
            }
    }

    public Slot GetSlotAt(SlotPosition pos)
    {
        return _slots[pos.X, pos.Y];
    }

    public bool IsValidPosition(SlotPosition pos)
    {
        return pos.X >= 0 && pos.X < _width && pos.Y >= 0 && pos.Y < _height;
    }

    public bool TryGetEmptyPosition(out SlotPosition position)
    {
        if (_emptyPositions.Count == 0)
        {
            position = default;
            return false;
        }

        position = _emptyPositions[Random.Range(0, _emptyPositions.Count)];
        return true;
    }

    public void PlaceItem(SlotPosition pos, ItemData data, IItemView view)
    {
        _slots[pos.X, pos.Y].Place(data, view);
        _emptyPositions.Remove(pos);
    }

    public void RemoveItem(SlotPosition pos)
    {
        _slots[pos.X, pos.Y].Clear();
        _emptyPositions.Add(pos);
    }

    public void MoveItem(SlotPosition from, SlotPosition to)
    {
        var fromSlot = _slots[from.X, from.Y];
        _slots[to.X, to.Y].Place(fromSlot.Data.Value, fromSlot.View);
        _slots[from.X, from.Y].Clear();

        _emptyPositions.Add(from);
        int index = _emptyPositions.IndexOf(to);
        if (index >= 0)
        {
            _emptyPositions[index] = _emptyPositions[^1];
            _emptyPositions.RemoveAt(_emptyPositions.Count - 1);
        }
    }
}

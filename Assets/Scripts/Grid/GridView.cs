using System.Collections.Generic;
using UnityEngine;

public class GridView
{
    private readonly BoardItemConfig _database;
    private SlotView[,] _slotViews;
    private int _width;
    private int _height;

    public GridView(BoardItemConfig database)
    {
        _database = database;
    }

    public void GenerateSlots(GridSize gridSize, Transform parent)
    {
        _width = gridSize.Width;
        _height = gridSize.Height;
        _slotViews = new SlotView[_width, _height];

        var slotPrefab = _database.TilePrefab;
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                var pos = new SlotPosition(x, y);
                var slotView = Object.Instantiate(slotPrefab, pos.ToWorldPosition(), Quaternion.identity, parent);
                _slotViews[x, y] = slotView;
            }
        }
    }

    public void RegisterItemView(SlotPosition pos, IItemView view)
    {
        _slotViews[pos.X, pos.Y].SetItemView(view);
    }

    public void UnregisterItemView(SlotPosition pos)
    {
        _slotViews[pos.X, pos.Y].ClearItemView();
    }

    public void MoveItemView(SlotPosition from, SlotPosition to)
    {
        var view = _slotViews[from.X, from.Y].ItemView;
        _slotViews[from.X, from.Y].ClearItemView();
        _slotViews[to.X, to.Y].SetItemView(view);
    }

    public IItemView GetItemViewAt(SlotPosition pos)
    {
        return _slotViews[pos.X, pos.Y].ItemView;
    }

    public void HighlightPositions(IReadOnlyList<SlotPosition> positions)
    {
        foreach (var pos in positions)
            _slotViews[pos.X, pos.Y].SetHighlight(Color.green);
    }

    public void ResetAllHighlights()
    {
        for (int x = 0; x < _width; x++)
            for (int y = 0; y < _height; y++)
                _slotViews[x, y].ResetHighlight();
    }
}

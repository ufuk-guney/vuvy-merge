using System.Collections.Generic;
using UnityEngine;

public class TileHandler
{
    private readonly BoardItemConfig _database;
    private readonly List<GameObject> _spawnedTiles = new();

    public TileHandler(BoardItemConfig database)
    {
        _database = database;
    }

    public void GenerateTiles(GridData gridData, Transform parent)
    {
        var tilePrefab = _database.TilePrefab;
        for (int y = 0; y < gridData.Height; y++)
        {
            for (int x = 0; x < gridData.Width; x++)
            {
                var position = new Vector3(x , y, 0f);
                var tile = Object.Instantiate(tilePrefab, position, Quaternion.identity, parent);
                _spawnedTiles.Add(tile);
            }
        }
    }

}

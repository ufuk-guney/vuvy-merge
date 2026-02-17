using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardItemDatabase", menuName = "Game/Board Item Database")]
public class BoardItemConfig : ScriptableObject
{
    public TileView TilePrefab;
    public ItemView ItemPrefab;
    public List<ItemChainData> ItemChainDataList;
}

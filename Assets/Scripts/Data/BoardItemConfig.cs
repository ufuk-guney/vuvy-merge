using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardItemDatabase", menuName = "Game/Board Item Database")]
public class BoardItemConfig : ScriptableObject
{
    public TileView TilePrefab;
    public GridItem ItemPrefab;
    public List<ItemChainData> ItemChainDataList;
}

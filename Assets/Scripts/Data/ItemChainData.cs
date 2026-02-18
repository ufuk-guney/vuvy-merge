using System.Collections.Generic;
using UnityEngine;

namespace VuvyMerge.Data
{
    [CreateAssetMenu(fileName = "ItemChainData", menuName = "Game/Item Chain Data")]
    public class ItemChainData : ScriptableObject
    {
        public ItemChainType ChainType;
        public List<Sprite> Sprites;

        public int MaxLevel => Sprites.Count - 1;
    }
}

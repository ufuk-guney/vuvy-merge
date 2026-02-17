public struct ItemData
{
    public ItemChainType ChainType;
    public int Level;

    public ItemData(ItemChainType chainType, int level)
    {
        ChainType = chainType;
        Level = level;
    }
}

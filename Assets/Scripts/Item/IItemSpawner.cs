public interface IItemSpawner
{
    void SpawnItem(ItemChainType chainType, int level, SlotPosition pos);
    void ReturnView(IItemView view);
}

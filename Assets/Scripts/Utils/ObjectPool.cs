using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly Stack<T> _pool = new();
    private readonly int _initialSize;

    public ObjectPool(T prefab, Transform parent, int initialSize)
    {
        _prefab = prefab;
        _parent = parent;
        _initialSize = initialSize;
    }

    public void Prewarm()
    {
        for (int i = 0; i < _initialSize; i++)
            _pool.Push(CreateInstance());
    }

    public T Get()
    {
        var item = _pool.Count > 0 ? _pool.Pop() : CreateInstance();
        item.gameObject.SetActive(true);
        return item;
    }

    public void Return(T item)
    {
        item.gameObject.SetActive(false);
        _pool.Push(item);
    }

    private T CreateInstance()
    {
        var obj = Object.Instantiate(_prefab, _parent);
        obj.gameObject.SetActive(false);
        return obj;
    }
}

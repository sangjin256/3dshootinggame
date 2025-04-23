using UnityEngine;
using System.Collections.Generic;

public class GameObjectPool<T> where T : MonoBehaviour, IPoolable
{
    private Queue<T> _poolQueue = new Queue<T>();

    private GameObject _prefab;

    public GameObjectPool(GameObject prefab, int capacity)
    {
        _prefab = prefab;
        for (int i = 0; i < capacity; i++)
        {
            T instance = GameObject.Instantiate(_prefab).GetComponent<T>();
            instance.SetPoolReference(this);
            instance.gameObject.SetActive(false);
        }
    }

    public T Get()
    {
        if(_poolQueue.Count > 0)
        {
            T obj = _poolQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            T newObj = GameObject.Instantiate(_prefab).GetComponent<T>();
            newObj.SetPoolReference(this);
            return newObj;
        }
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        _poolQueue.Enqueue(obj);
    }
}

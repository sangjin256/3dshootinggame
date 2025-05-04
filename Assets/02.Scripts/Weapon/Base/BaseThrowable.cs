using UnityEngine;

public abstract class BaseThrowable : MonoBehaviour, IThrowable, IPoolable
{
    private GameObjectPool<BaseThrowable> _thisPool;

    public void SetPoolReference<T>(GameObjectPool<T> pool) where T : MonoBehaviour, IPoolable
    {
        _thisPool = pool as GameObjectPool<BaseThrowable>;
    }
    public void ReturnToPool()
    {
        _thisPool?.ReturnToPool(this);
    }
}

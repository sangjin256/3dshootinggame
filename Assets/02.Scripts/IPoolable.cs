using UnityEngine;

public interface IPoolable
{
    void SetPoolReference<T>(GameObjectPool<T> pool) where T : MonoBehaviour, IPoolable;
    void ReturnToPool();
}

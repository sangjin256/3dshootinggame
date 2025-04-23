using UnityEngine;

public class BulletTrail : MonoBehaviour, IPoolable
{
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private float _progress = 0f;

    private float _speed = 40f;
    private GameObjectPool<BulletTrail> _thisPool;

    public void Initialize(Vector3 startPosition, Vector3 targetPosition)
    {
        _startPosition = startPosition;
        _targetPosition = targetPosition;
        _progress = 0f;
        transform.position = _startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        _progress += Time.deltaTime * _speed;
        transform.position = Vector3.Lerp(_startPosition, _targetPosition, _progress);
        if (Vector3.Distance(_targetPosition, transform.position) <= 0.001f)
        {
            _progress = 0f;
            transform.position = _startPosition;
            ReturnToPool();
        }
    }
    public void SetPoolReference<T>(GameObjectPool<T> pool) where T : MonoBehaviour, IPoolable
    {
        _thisPool = pool as GameObjectPool<BulletTrail>;
    }
    public void ReturnToPool()
    {
        _thisPool?.ReturnToPool(this);
    }
}

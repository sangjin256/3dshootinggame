using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private float _progress = 0f;

    private float _speed = 40f;

    public void Initialize(Vector3 startPosition, Vector3 targetPosition)
    {
        _startPosition = startPosition;
        _targetPosition = targetPosition;
    }

    private void OnEnable()
    {
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
            gameObject.SetActive(false);

        }
    }
}

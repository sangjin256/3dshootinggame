using UnityEngine;

public class Coin : MonoBehaviour
{
    private float RotationY = 0;
    public float RotationSpeed;
    private bool isAttracted = false;
    private Transform playerTransform;
    public float attractionSpeed = 5f;
    private float destroyDistance = 0.5f;
    private float progress = 0f;
    private Vector3 randomDirection;

    void Update()
    {
        RotationY += Time.deltaTime * RotationSpeed;
        if (RotationY >= 360f) RotationY = 0f;
        transform.parent.rotation = Quaternion.Euler(new Vector3(0, RotationY, 0));

        if (isAttracted && playerTransform != null)
        {
            Vector3 StartPos = transform.position;
            Vector3 EndPos = playerTransform.position;
            Vector3 ControlPoint = (StartPos + EndPos) * 0.5f + randomDirection * 2f;

            // 진행도를 누적하여 계산
            progress += Time.deltaTime * attractionSpeed;
            progress = Mathf.Clamp01(progress);

            Vector3 newPosition = CalculateBezierPoint(progress, StartPos, ControlPoint, EndPos);
            transform.parent.position = newPosition;

            if (Vector3.Distance(transform.parent.position, playerTransform.position) < destroyDistance)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAttracted = true;
            playerTransform = other.transform;
            progress = 0f;
            
            randomDirection = Random.value > 0.5f ? Vector3.right : Vector3.left;
        }
    }
}

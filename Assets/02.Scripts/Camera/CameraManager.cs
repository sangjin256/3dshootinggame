using NUnit;
using System.Collections;
using UnityEngine;

public class CameraManager : BehaviourSingleton<CameraManager>
{
    public Transform FPSTarget;
    public Transform TPSTarget;

    public FPSCamera FPSCamera;
    public TPSCamera TPSCamera;

    private Vector3 originalPosition;
    public Vector3 ShakePosition;
    public float BoundY = 0.2f;

    public bool IsShooting = false;

    private void Awake()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Shake(float duration, float magnitude)
    {
        originalPosition = Camera.main.transform.position;
        ShakePosition = Vector3.zero;
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            ShakePosition = new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        ShakePosition = Vector3.zero;
        transform.localPosition = originalPosition;
    }
}

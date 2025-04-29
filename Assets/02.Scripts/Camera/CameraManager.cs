using DG.Tweening;
using NUnit;
using System.Collections;
using UnityEngine;

public class CameraManager : BehaviourSingleton<CameraManager>
{
    public Transform FPSTarget;
    public Transform TPSTarget;
    public Transform QuaterViewTarget;

    public FPSCamera FPSCamera;
    public TPSCamera TPSCamera;
    public QuarterViewCamera QVCamera;
    private Transform _player;

    private Vector3 originalPosition;
    public Vector3 ShakePosition;
    public float BoundY = 0.2f;

    public float RotationX;
    public float RotationY;

    public float ClampY;

    public float HorizontalSpeed = 150f;
    public float VerticalSpeed = 150f;
    public bool IsShooting = false;

    public float TransitionDuration;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        RotateHorizontal();
        RotateVertical();

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Cursor.lockState = CursorLockMode.Locked;
            if (FPSCamera.enabled == true) return;
            TPSCamera.enabled = false;
            QVCamera.enabled = false;

            transform.DORotate(_player.transform.eulerAngles, TransitionDuration);
            transform.DOMove(FPSCamera.GetPosition(), TransitionDuration).OnComplete(() => FPSCamera.enabled = true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Cursor.lockState = CursorLockMode.Locked;
            if (TPSCamera.enabled == true) return;
            FPSCamera.enabled = false;
            QVCamera.enabled = false;
            
            transform.DORotate(_player.transform.eulerAngles, TransitionDuration);
            transform.DOMove(TPSCamera.GetPosition(), TransitionDuration).OnComplete(() => TPSCamera.enabled = true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Cursor.lockState = CursorLockMode.None;
            if (QVCamera.enabled == true) return;
            FPSCamera.enabled = false;
            TPSCamera.enabled = false;

            transform.DOMove(QVCamera.GetPosition(), TransitionDuration).OnComplete(() => QVCamera.enabled = true);
        }
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

    public void RotateHorizontal()
    {
        float mouseX = Input.GetAxis("Mouse X");
        RotationX += mouseX * HorizontalSpeed * Time.deltaTime;
    }

    public void RotateVertical()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        RotationY += mouseY * VerticalSpeed * Time.deltaTime;
        RotationY = Mathf.Clamp(RotationY, -ClampY, ClampY);
    }

    public Vector3 GetFireDirection()
    {
        if (FPSCamera.enabled)
        {

        }
        else if (TPSCamera.enabled)
        {

        }
        else
        {

        }

        return Vector3.zero;
    }
}

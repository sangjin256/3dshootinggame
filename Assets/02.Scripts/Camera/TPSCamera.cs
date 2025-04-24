using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TPSCamera : MonoBehaviour
{
    public float HorizontalSpeed = 150f;
    public float VerticalSpeed = 20f;

    private float _rotationX = 0;
    private float _rotationY = 0;

    [SerializeField] private float _distance = 5;

    public void Initialize()
    {
        // TPS카메라는 이전 카메라의 방향 정보를 가져와서 그 방향을 가리키고 시작해야됨
    }

    private void Update()
    {
        RotateHorizontal();
        RotateVertical();

        transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);
        transform.position = CameraManager.I.ShakePosition + CameraManager.I.TPSTarget.position - transform.forward * _distance;
    }

    public void RotateHorizontal()
    {
        float mouseX = Input.GetAxis("Mouse X");
        _rotationX += mouseX * HorizontalSpeed * Time.deltaTime;
    }

    public void RotateVertical()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationY += mouseY * VerticalSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);
    }
}

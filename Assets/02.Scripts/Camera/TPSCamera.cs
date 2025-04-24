using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TPSCamera : MonoBehaviour
{
    public float HorizontalSpeed = 150f;
    public float VerticalSpeed = 20f;

    private float _rotationX = 0;
    private float _rotationY = 0;

    [SerializeField] private float _distance = 5;

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

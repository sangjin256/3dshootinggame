using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TPSCamera : MonoBehaviour, ICameraComponent
{
    private CameraController _controller;
    public float RotationSpeed = 150f;

    private float _rotationX = 0;
    private float _rotationY = 0;

    public void Initialize(CameraController controller)
    {
        _controller = controller;
    }

    private void Update()
    {
        RotateHorizontal();
        RotateVertical();

        transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);
    }

    private void LateUpdate()
    {
        transform.position = _controller.TPSTarget.position;
    }

    public void RotateHorizontal()
    {
        float mouseX = Input.GetAxis("Mouse X");
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
    }

    public void RotateVertical()
    {

        float mouseY = Input.GetAxis("Mouse Y");

        _rotationY += mouseY * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);
    }
}

using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TPSCamera : MonoBehaviour, ICameraComponent
{
    private CameraController _controller;
    public float HorizontalSpeed = 150f;
    public float VerticalSpeed = 20f;

    private float _rotationX = 0;
    private float _rotationY = 0;

    [SerializeField] private float _distance = 0;

    public void Initialize(CameraController controller)
    {
        _controller = controller;
        _distance = Vector3.Distance(_controller.TPSTarget.position, _controller.TPSTarget.parent.position);
    }

    private void Update()
    {
        RotateHorizontal();
        RotateVertical();

        transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);
    }

    private void LateUpdate()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        float y = Mathf.Cos(-_rotationY) * _distance;
        float z = Mathf.Sin(-_rotationY) * _distance;

        
        transform.position = _controller.TPSTarget.parent.position + new Vector3(_controller.TPSTarget.position.x, y, z);
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
        _rotationY = Mathf.Clamp(_rotationY, -70f, 70f);
    }
}

using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    public float RotationSpeed = 150f;

    // 카메라 각도는 0도에서부터 시작한다고 기준을 세운다.
    private float _rotationX = 0;
    private float _rotationY = 0;

    private void Update()
    {
        // 1. 마우스 입력을 받는다. (마우스 커서의 움직임 방향)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. 마우스 입력으로부터 회전시킬 방향을 만든다.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY += mouseY * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        if (CameraManager.I.IsShooting) _rotationY += CameraManager.I.BoundY;

        transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);
    }

    private void LateUpdate()
    {
        transform.position = CameraManager.I.ShakePosition + CameraManager.I.FPSTarget.position;
    }


}

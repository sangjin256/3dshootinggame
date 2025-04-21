using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // 카메라 회전 스크립트
    // 목표 : 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.
    // 구현 순서
    // 1. 마우스 입력을 받는다.
    // 2. 마우스 입력으로부터 회전시킬 방향을 만든다.
    // 3. 카메라를 그 방향으로 회전한다.

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

        transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.LookAt(Vector3.zero);
            _rotationX = 0;
            _rotationY = 0;
        }
    }
}

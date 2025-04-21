using UnityEngine;

public class PlayerRotate : MonoBehaviour , IPlayerComponent
{
    public float RotationSpeed = 150f; // 카메라와 회전속도가 똑같아야 한다.
    private float _rotationX = 0;
    private PlayerController _controller;
        
    public void Initialize(PlayerController controller)
    {
        _controller = controller;
    }

    private void Update()
    {
        // 1. 마우스 입력을 받는다. (마우스 커서의 움직임 방향)
        float mouseX = Input.GetAxis("Mouse X");

        // 2. 마우스 입력으로부터 회전시킬 방향을 만든다.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, _rotationX, 0);
    }
}

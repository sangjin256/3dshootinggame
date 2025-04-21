using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 목표 : wasd를 누르면 캐릭터를 '카메라 방향에 맞게' 이동시키고 싶다.

    // 구현 순서 :
    // 1. 키보드 입력을 받는다.
    // 2. 입력으로부터 방향을 설정한다.
    // 3. 방향에 따라 플레이어를 이동한다.

    public float MoveSpeed = 7f;
    public float JumpPower = 5f;

    private const float GRAVITY = -9.8f;  // 중력
    private float _yVelocity = 0f;        // 중력가속도

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 메인카메라를 기준으로 방향을 변환한다.
        dir = transform.TransformDirection(dir);
        // TransformDirection : 지역 공간의 벡터를 월드 공간의 벡터로 바꿔주는 함수

        // 중력 적용
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;

        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVelocity = JumpPower;
        }

        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerMove : MonoBehaviour
{
    // 목표 : wasd를 누르면 캐릭터를 '카메라 방향에 맞게' 이동시키고 싶다.

    // 구현 순서 :
    // 1. 키보드 입력을 받는다.
    // 2. 입력으로부터 방향을 설정한다.
    // 3. 방향에 따라 플레이어를 이동한다.

    private float _originMoveSpeed = 5f;
    public float MoveSpeed = 5f;
    public float JumpPower = 5f;

    public int JumpCount = 0;
    private bool _isClimbing = false;

    private const float GRAVITY = -9.8f;  // 중력
    private float _yVelocity = 0f;        // 중력가속도

    private CharacterController _characterController;

    private Vector3 _direction;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        _direction = new Vector3(h, 0, v);
        _direction = _direction.normalized;

        // 메인카메라를 기준으로 방향을 변환한다.
        _direction = transform.TransformDirection(_direction);
        // Transform_directionection : 지역 공간의 벡터를 월드 공간의 벡터로 바꿔주는 함수


        Jump();

        // 중력 적용
        if (_characterController.isGrounded == false)
        {
            _yVelocity += GRAVITY * Time.deltaTime;
            _direction.y = _yVelocity;
        }
        else
        {
            JumpCount = 0;
            if (_isClimbing)
            {
                // yVelocity가 필드 위에서 계속 -로 가는 문제있음
                _yVelocity = 0;
                _isClimbing = false;
                Player.I.IsUsingStamina = false;
            }
        }


        Sprint();

        if (Input.GetKeyDown(KeyCode.E)) Dash();
        if (v > 0f && ((int)_characterController.collisionFlags & (int)CollisionFlags.Sides) != 0) Climb();
        else
        {
            _characterController.Move(_direction * MoveSpeed * Time.deltaTime);
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (JumpCount < 2)
            {
                _yVelocity = JumpPower;
                JumpCount++;
            }
        }
    }

    public void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _characterController.isGrounded)
        {
            if (Player.I.IsExhausted == false)
            {
                MoveSpeed = 12f;
                Player.I.IsUsingStamina = true;
                Player.I.UseStamina(12f * Time.deltaTime);
            }
            else
            {
                MoveSpeed = _originMoveSpeed;
                Player.I.IsUsingStamina = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && MoveSpeed > _originMoveSpeed)
        {
            MoveSpeed = _originMoveSpeed;
            Player.I.IsUsingStamina = false;
        }
    }

    public void Dash()
    {
        if (Player.I.IsExhausted == false)
        {
            _characterController.Move(transform.TransformDirection(_direction + Vector3.forward * 5f));
            Player.I.UseStamina(20f);
        }
    }

    public void Climb()
    {
        Player.I.UseStamina(12f * Time.deltaTime);
        if (Player.I.IsExhausted == false)
        {
            _yVelocity = 0;
            _isClimbing = true;
            _characterController.Move(transform.TransformDirection((new Vector3(0,1,1)) * MoveSpeed * Time.deltaTime));
            Player.I.IsUsingStamina = true;
        }
        else
        {
            _characterController.Move(_direction * MoveSpeed * Time.deltaTime);
        }
    }
}

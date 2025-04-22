using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerMove : MonoBehaviour, IPlayerComponent
{
    // 목표 : wasd를 누르면 캐릭터를 '카메라 방향에 맞게' 이동시키고 싶다.

    // 구현 순서 :
    // 1. 키보드 입력을 받는다.
    // 2. 입력으로부터 방향을 설정한다.
    // 3. 방향에 따라 플레이어를 이동한다.

    [SerializeField] private float _originMoveSpeed = 5f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private int _jumpCount = 0;

    [SerializeField] private bool _isClimbing = false;
    [SerializeField] private bool _isRolling = false;

    private float _rollTime = 0f;
    private float _rollDuration = 0.2f;  // 대쉬 지속 시간
    private float _rollSpeed = 30f;  // 대쉬 속도

    private const float GRAVITY = -9.8f;  // 중력
    private float _yVelocity = 0f;        // 중력가속도

    private CharacterController _characterController;
    private PlayerController _controller;

    private Vector3 _direction;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void Initialize(PlayerController controller)
    {
        _controller = controller;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if(!_isRolling && !_isClimbing) _direction = new Vector3(h, -0.01f, v).normalized;

        #region 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        #endregion

        #region 스프린트
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Sprint();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && _moveSpeed > _originMoveSpeed)
        {
            EndBehaviour();
        }
        #endregion

        #region 대쉬
        if (_isRolling)
        {
            if(_rollTime >= _rollDuration)
            {
                _rollTime = 0;
                _isRolling = false;
                EndBehaviour();
            }
            else
            {
                _direction = Vector3.forward;
            }
            _rollTime += Time.deltaTime;
        }

        if (!_isRolling && Input.GetKeyDown(KeyCode.E)) Roll();
        #endregion

        #region 벽타기
        if (v > 0f && ((int)_characterController.collisionFlags & (int)CollisionFlags.Sides) != 0) Climb();
        if ((_characterController.collisionFlags & CollisionFlags.Sides) == 0)
        {
            _isClimbing = false;
        }
        #endregion

        if (!_isClimbing)
        {
            _yVelocity += GRAVITY * Time.deltaTime;
            _direction.y = _yVelocity;
        }

        _direction = transform.TransformDirection(_direction);
        _characterController.Move(new Vector3(_direction.x * _moveSpeed, _direction.y * _originMoveSpeed, _direction.z * _moveSpeed) * Time.deltaTime);

        if((_characterController.collisionFlags & CollisionFlags.Below) != 0)
        {
            _jumpCount = 0;
            _yVelocity = 0f;
        }
        

    }

    public void Jump()
    {
        if (_jumpCount < 2)
        {
            _yVelocity = _jumpPower;
            _jumpCount++;
        }
    }

    public void Sprint()
    {
        if (_controller.IsExhausted == false)
        {
            _moveSpeed = 12f;
            _controller.UseStamina(12f * Time.deltaTime);

            _controller.IsUsingStamina = true;
        }
        else
        {
            EndBehaviour();
        }
    }

    public void EndBehaviour()
    {
        _moveSpeed = _originMoveSpeed;
        _controller.IsUsingStamina = false;
    }

    public void Roll()
    {
        if (_controller.IsExhausted == false)
        {
            _isRolling = true;
            _direction = Vector3.forward;
            _moveSpeed = _rollSpeed;
            _controller.UseStamina(20f);
        }
    }

    public void Climb()
    {
        if (_controller.IsExhausted == false)
        {
            _yVelocity = 0;
            _isClimbing = true;
            _direction = new Vector3(0, 1, .7f).normalized;
            _controller.UseStamina(12f * Time.deltaTime);
            _controller.IsUsingStamina = true;
        }
        else
        {
            _isClimbing = false;
            _controller.IsUsingStamina = false;
        }
    }
}

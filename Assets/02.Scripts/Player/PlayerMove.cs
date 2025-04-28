using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem.XR;

public class PlayerMove : APlayerComponent
{
    [SerializeField] private float _originMoveSpeed = 5f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private int _jumpCount = 0;

    [SerializeField] private bool _isClimbing = false;
    [SerializeField] private bool _isRolling = false;

    private float _rollTime = 0f;
    private float _rollDuration = 0.2f;
    private float _rollSpeed = 30f;

    private const float GRAVITY = -9.8f;
    private float _yVelocity = 0f;

    private CharacterController _characterController;

    private Vector3 _direction;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (!_isRolling && !_isClimbing) _direction = new Vector3(h, -0.001f, v);

        Jump();
        Sprint();
        Roll();

        if(v > 0) Climb();
        if ((_characterController.collisionFlags & CollisionFlags.Sides) == 0)
        {
            _isClimbing = false;
        }
        if (!_isClimbing)
        {
            _yVelocity += GRAVITY * Time.deltaTime;
            _direction.y = _yVelocity;
        }


        _controller.Animator.SetFloat("MoveAmount", _direction.magnitude);
        _direction = _direction.normalized;
        if (!CameraManager.I.QVCamera.enabled || _isRolling) _direction = transform.TransformDirection(_direction);
        _characterController.Move(new Vector3(_direction.x * _moveSpeed, _direction.y * _originMoveSpeed, _direction.z * _moveSpeed) * Time.deltaTime);


        if((_characterController.collisionFlags & CollisionFlags.Below) != 0)
        {
            _jumpCount = 0;
            _yVelocity = 0f;
        } 
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_jumpCount < 2)
            {
                _yVelocity = _jumpPower;
                _jumpCount++;
            }
        }
    }

    public void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!_controller.IsExhausted)
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

        if (Input.GetKeyUp(KeyCode.LeftShift)) EndBehaviour();
    }

    public void EndBehaviour()
    {
        _moveSpeed = _originMoveSpeed;
        _controller.IsUsingStamina = false;
    }

    public void Roll()
    {
        if (_isRolling)
        {
            if (_rollTime >= _rollDuration)
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

        if (!_isRolling && Input.GetKeyDown(KeyCode.E))
        {
            if (!_controller.IsExhausted)
            {
                _isRolling = true;
                _direction = transform.forward;
                _moveSpeed = _rollSpeed;
                _controller.UseStamina(20f);
            }
        }
    }

    public void Climb()
    {
        if (((int)_characterController.collisionFlags & (int)CollisionFlags.Sides) != 0)
        {
            if (!_controller.IsExhausted)
            {
                _yVelocity = 0;
                _isClimbing = true;
                _direction = new Vector3(0, 1, .7f);
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
}

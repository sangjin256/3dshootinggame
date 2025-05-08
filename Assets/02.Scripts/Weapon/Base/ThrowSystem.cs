using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ThrowSystem : MonoBehaviour, IWeapon
{
    public int ThrowableCount;
    public int MaxThrowableCount;
    public bool IsCharging;
    public float ChargeSpeed;

    protected float _throwPower;
    public float StartThrowPower;
    public float MaxThrowPower;

    public GameObject ThrowablePrefab;
    public Transform OriginPosition;
    public GameObject GrenadeOnHand;

    public GameObjectPool<BaseThrowable> ThrowablePool;

    private void Start()
    {
        ThrowablePool = new GameObjectPool<BaseThrowable>(ThrowablePrefab, MaxThrowableCount * 2);
    }

    private void LateUpdate()
    {
        //PositionByCamera();
    }

    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ThrowableCount > 0)
            {
                IsCharging = true;
                _throwPower = StartThrowPower;
            }
        }

        if (IsCharging) Charge();

        if (Input.GetMouseButtonUp(0))
        {
            if (ThrowableCount > 0)
            {
                GameManager.Instance.Player.Animator.SetTrigger("Toss");
            }
        }
    }

    public void PositionByCamera()
    {
    }

    public void Charge()
    {
        _throwPower += Time.deltaTime * ChargeSpeed;
        if (_throwPower >= MaxThrowPower) _throwPower = MaxThrowPower;
    }

    public void Throw()
    {
        GameObject throwable = ThrowablePool.Get().gameObject;
        ThrowableCount--;
        PlayerEventManager.Instance.OnThrow?.Invoke();
        throwable.transform.position = OriginPosition.position;
        // 풀에서 가져오는거라 초기화 필요
        //throwable.transform.position = FirePosition.transform.position;

        // 4. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기
        Rigidbody bombRigidbody = throwable.GetComponent<Rigidbody>();
        bombRigidbody.AddForce(Camera.main.transform.forward * _throwPower, ForceMode.Impulse);
        bombRigidbody.AddTorque(Vector3.one);

        _throwPower = 0f;
        IsCharging = false;
    }

    public void Enter()
    {
        GrenadeOnHand.SetActive(true);
        GameManager.Instance.Player.Animator.SetLayerWeight(GameManager.Instance.Player.Animator.GetLayerIndex("ThrowLayer"), 1);
    }

    public void Exit()
    {
        GrenadeOnHand.SetActive(false);
        GameManager.Instance.Player.Animator.SetLayerWeight(GameManager.Instance.Player.Animator.GetLayerIndex("ThrowLayer"), 0);
    }
}

using UnityEngine;

public abstract class BaseMelee : MonoBehaviour, IMeleeable, IWeapon
{
    public Transform PlayerTransform;

    public int Damage;
    public float HorizontalAngle;
    public float Range;
    private float _rangeCosValue;
    public float VerticalRange;
    public float AttackCooltime;
    private float _attackElapsedtime = 0;
    protected bool _isAnimation = false;

    public LayerMask LayerMask;
    public Vector3 _weaponOffset;

    public Transform AttackPoint;

    public virtual void Awake()
    {
        _rangeCosValue = Mathf.Cos(Range/2);
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void LateUpdate()
    {
        PositionByCamera();
    }

    public virtual void HandleInput()
    {
        _attackElapsedtime += Time.deltaTime;
        if(_attackElapsedtime >= AttackCooltime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                _attackElapsedtime = 0;
            }
            else
            {
                // 가만히있을때 숫자 계속 커지는거 방지
                _attackElapsedtime = AttackCooltime + 1f;
            }
        }

    }

    public virtual void PositionByCamera()
    {

        if (CameraManager.I.FPSCamera.enabled)
        {
            if (_isAnimation) return;
            transform.position = Camera.main.transform.position + Camera.main.transform.TransformDirection(_weaponOffset);
            transform.rotation = Camera.main.transform.rotation;

            transform.position += CameraManager.I.ShakePosition;
        }
        else if (CameraManager.I.TPSCamera.enabled)
        {
            if (!_isAnimation) transform.localPosition = _weaponOffset;
            transform.forward = Camera.main.transform.forward;
        }
        else
        {
            Vector3 mouseDirection = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0f);
            mouseDirection = mouseDirection.normalized;
            transform.forward = new Vector3(mouseDirection.x, 0, mouseDirection.y);
        }
    }

    public virtual void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(PlayerTransform.position, Range, LayerMask);

        AttackAnimation();

        for(int i = 0; i < colliders.Length; i++)
        {
            Vector3 targetPosition = colliders[i].transform.position;
            targetPosition = new Vector3(targetPosition.x, AttackPoint.position.y, targetPosition.z);
            Vector3 directionToTarget = (targetPosition - AttackPoint.position).normalized;
            Debug.Log(Vector3.Dot(AttackPoint.forward, directionToTarget));
            float cosValue = Vector3.Dot(AttackPoint.forward, directionToTarget);
            if(cosValue >= _rangeCosValue)
            {
                if (colliders[i].transform.position.y >= AttackPoint.position.y - VerticalRange && colliders[i].transform.position.y <= AttackPoint.position.y + VerticalRange)
                {
                    Damage damage = new Damage();
                    damage.Value = Damage;
                    damage.From = this.gameObject;
                    colliders[i].GetComponent<IDamageable>().TakeDamage(damage);
                }
            }
        }
    }

    public abstract void AttackAnimation();
}

using UnityEngine;

public abstract class BaseMelee : MonoBehaviour, IMeleeable, IWeapon
{
    public Transform PlayerTransform;

    public int Damage;
    public float HorizontalAngle;
    public float Range;
    public float VerticalRange;
    public LayerMask LayerMask;

    private float _rangeCosValue;

    public Transform AttackPoint;

    public virtual void Start()
    {
        _rangeCosValue = Mathf.Cos(Range/2);
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(PlayerTransform.position, Range, LayerMask);

        for(int i = 0; i < colliders.Length; i++)
        {
            Vector3 targetPosition = colliders[i].transform.position;
            targetPosition = new Vector3(targetPosition.x, AttackPoint.position.y, targetPosition.z);
            Vector3 directionToTarget = (targetPosition - AttackPoint.position).normalized;
            Debug.Log(Vector3.Dot(AttackPoint.forward, directionToTarget));
            float cosValue = Vector3.Dot(AttackPoint.forward, directionToTarget);
            DrawSector(PlayerTransform.position, AttackPoint.forward, Range, HorizontalAngle * 2, 50, Color.red);
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

    public virtual void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void DrawSector(Vector3 origin, Vector3 forward, float radius, float angle, int segments, Color color)
    {
        // 중심 방향 기준 시작각
        float halfAngle = angle / 2f;
        Quaternion startRotation = Quaternion.AngleAxis(-halfAngle, Vector3.up);
        Vector3 startDir = startRotation * forward.normalized;

        // 각 분할 간격
        float deltaAngle = angle / segments;

        Vector3 prevPoint = origin + startDir * radius;
        Debug.DrawLine(origin, prevPoint, color);

        for (int i = 1; i <= segments; i++)
        {
            Quaternion rot = Quaternion.AngleAxis(deltaAngle * i - halfAngle, Vector3.up);
            Vector3 currentDir = rot * forward.normalized;
            Vector3 currentPoint = origin + currentDir * radius;

            Debug.DrawLine(prevPoint, currentPoint, color); // 외곽 연결
            Debug.DrawLine(origin, currentPoint, color);    // 중심에서 각 포인트까지

            prevPoint = currentPoint;
        }
    }
}

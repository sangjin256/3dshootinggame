using UnityEngine;

public class SMG : BaseFirearm
{
    private Transform _cameraTransform;
    [SerializeField] private Vector3 _weaponOffset = new Vector3(0.2f, -0.1f, 0.3f); // 적절한 오프셋 값으로 조정
    private Vector3 _lastCameraPosition;
    private Quaternion _lastCameraRotation;
    private float _rotationSmoothSpeed = 15f;  // 회전 속도 조절 값

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraPosition = _cameraTransform.position;
        _lastCameraRotation = _cameraTransform.rotation;
    }

    private void LateUpdate()
    {
        if (CameraManager.I.FPSCamera.enabled)
        {
            // FPS 모드일 때는 카메라를 정확히 따라감
            transform.position = _cameraTransform.position + _cameraTransform.TransformDirection(_weaponOffset);
            transform.rotation = _cameraTransform.rotation;
            
            // 카메라 흔들림 효과 적용
            transform.position += CameraManager.I.ShakePosition;
        }
        else
        {
            // TPS 모드일 때는 플레이어의 자식으로서의 위치 유지
            transform.localPosition = _weaponOffset;
            transform.forward = _cameraTransform.forward;
        }

        _lastCameraPosition = _cameraTransform.position;
        _lastCameraRotation = _cameraTransform.rotation;
    }

    public override void HandleFireInput()
    {
        if (Input.GetMouseButton(0)) Fire();
        if (Input.GetMouseButtonUp(0))
        {
            FireElapsedTime = FireCoolTime;
            SpreadElapsedTime = 0f;
            CameraManager.I.IsShooting = false;
        }

        if (Input.GetKeyDown(KeyCode.R)) Reload();
    }

    public override void Fire()
    {
        if (CurrentAmmo > 0)
        {
            CameraManager.I.IsShooting = true;
            if (FireElapsedTime >= FireCoolTime)
            {
                Vector3 finalDireciton = RandomSpreadDirection();
                Ray ray = new Ray(FirePosition.position, finalDireciton);
                RaycastHit hitInfo = new RaycastHit();

                bool isHit = Physics.Raycast(ray, out hitInfo, 50f);
                if (isHit)
                {
                    CurrentAmmo--;
                    PlayerEventManager.I.OnFire?.Invoke();
                    OnHitEffect(hitInfo, 0.1f);
                    if (hitInfo.collider.GetComponent<IDamageable>() != null)
                    {
                        IDamageable target = hitInfo.collider.GetComponent<IDamageable>();
                        Damage damage = new Damage();
                        damage.Value = DamageAmount;
                        damage.From = this.gameObject;

                        target.TakeDamage(damage);
                        Knockback(hitInfo.collider.gameObject, finalDireciton);
                    }
                }
                else
                {
                    CurrentAmmo--;
                    PlayerEventManager.I.OnFire?.Invoke();
                    CameraManager.I.Shake(0.1f, 0.1f);
                    BulletTrail trail = BulletTrailPool.Get();
                    trail.Initialize(FirePosition.transform.position, FirePosition.transform.position + finalDireciton.normalized * 30f);
                }

                FireElapsedTime = 0;
            }
            //else
            //{
            //    CameraManager.I.IsShooting = false;
            //}

            SpreadElapsedTime += Time.deltaTime;
            if (SpreadElapsedTime + 0.5f >= SpreadCoolTime)
            {
                SpreadElapsedTime = SpreadCoolTime - 0.5f;
            }
        }
        else
        {
            CameraManager.I.IsShooting = false;
        }

        FireElapsedTime += Time.deltaTime;

        if (IsReloading)
        {
            IsReloading = false;
            StopAllCoroutines();
            PlayerEventManager.I.OnReload(false);
        }
    }

    public override void Reload()
    {
        base.Reload();
    }
}

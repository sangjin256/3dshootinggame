using UnityEngine;

public class SMG : BaseFirearm
{

    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
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

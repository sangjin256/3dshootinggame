using DG.Tweening;
using UnityEngine;

public class SMG : BaseFirearm
{
    private Transform _cameraTransform;
    

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    public override void Fire()
    {
        if (CurrentAmmo > 0)
        {
            CameraManager.Instance.IsShooting = true;
            if (FireElapsedTime >= FireCoolTime)
            {
                Vector3 finalDireciton = RandomSpreadDirection();
                Ray ray = new Ray(CameraManager.Instance.transform.position, finalDireciton);

                if (CameraManager.Instance.QVCamera.enabled)
                {
                    Vector2 randomPosition = Random.insideUnitCircle * ((SpreadCoolTime - SpreadElapsedTime) / SpreadCoolTime) * SpreadAmount;
                    finalDireciton = (FirePosition.forward + new Vector3(randomPosition.x, randomPosition.y, 0)).normalized;
                    ray = new Ray(FirePosition.position, finalDireciton);
                }

                RaycastHit hitInfo = new RaycastHit();

                bool isHit = Physics.Raycast(ray, out hitInfo, 50f, LayerMask);
                if (isHit)
                {
                    CurrentAmmo--;
                    PlayerEventManager.Instance.OnFire?.Invoke();
                    OnHitEffect(hitInfo, 0.05f);
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
                    PlayerEventManager.Instance.OnFire?.Invoke();
                    CameraManager.Instance.Shake(0.1f, 0.05f);

                    BulletTrail trail = BulletTrailPool.Get();
                    trail.Initialize(FirePosition.position, FirePosition.position + finalDireciton.normalized * 30f);
                }

                MuzzleParticle.Play();
                FireElapsedTime = 0;
            }

            SpreadElapsedTime += Time.deltaTime;
            if (SpreadElapsedTime + 0.5f >= SpreadCoolTime)
            {
                SpreadElapsedTime = SpreadCoolTime - 0.5f;
            }
        }
        else
        {
            CameraManager.Instance.IsShooting = false;
        }

        FireElapsedTime += Time.deltaTime;

        if (IsReloading)
        {
            IsReloading = false;
            StopAllCoroutines();
            PlayerEventManager.Instance.OnReload(false);
        }
    }

    public override void Reload()
    {
        base.Reload();
    }
}

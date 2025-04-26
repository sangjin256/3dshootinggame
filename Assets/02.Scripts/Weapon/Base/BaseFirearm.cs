using UnityEngine;
using System.Collections;

public abstract class BaseFirearm : MonoBehaviour, IFireable, IWeapon
{
    public int MaxAmmo;
    public int CurrentAmmo;

    public float FireCoolTime;
    public float FireElapsedTime;

    public float SpreadCoolTime;
    public float SpreadElapsedTime;
    public float SpreadAmount;

    public float KnockbackElapsedTime;
    public float KnockbackTimer;
    public float KnockbackPower;

    public bool IsReloading;
    public int DamageAmount;

    public Transform FirePosition;
    public ParticleSystem BulletEffect;
    public GameObject BulletTrailPrefab;
    public GameObjectPool<BulletTrail> BulletTrailPool;

    public Vector3 _weaponOffset;

    private void Awake()
    {
        BulletTrailPool = new GameObjectPool<BulletTrail>(BulletTrailPrefab, MaxAmmo / 2);
    }

    private void LateUpdate()
    {
        PositionByCamera();
    }

    public virtual void HandleInput()
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

    public virtual void PositionByCamera()
    {

        if (CameraManager.I.FPSCamera.enabled)
        {
            transform.position = Camera.main.transform.position + Camera.main.transform.TransformDirection(_weaponOffset);
            transform.rotation = Camera.main.transform.rotation;

            transform.position += CameraManager.I.ShakePosition;
        }
        else if (CameraManager.I.TPSCamera.enabled)
        {
            transform.localPosition = _weaponOffset;
            transform.forward = Camera.main.transform.forward;
        }
        else
        {
            Vector3 mouseDirection = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0f);
            mouseDirection = mouseDirection.normalized;
            transform.forward = new Vector3(mouseDirection.x, 0, mouseDirection.y);
        }
    }

    public abstract void Fire();

    public virtual void Reload()
    {
        if (IsReloading) return;

        if (CurrentAmmo < MaxAmmo)
        {
            StartCoroutine(ReLoadCoroutine());
        }
    }

    private IEnumerator ReLoadCoroutine()
    {
        PlayerEventManager.I.OnReload?.Invoke(true);

        IsReloading = true;
        yield return new WaitForSeconds(2f);
        IsReloading = false;
        CurrentAmmo = MaxAmmo;

        PlayerEventManager.I.OnFire?.Invoke();
        PlayerEventManager.I.OnReload?.Invoke(false);
    }

    protected Vector3 RandomSpreadDirection()
    {
        Vector2 randomPosition = Random.insideUnitCircle * ((SpreadCoolTime - SpreadElapsedTime) / SpreadCoolTime) * SpreadAmount;
        Vector3 finalDireciton = (CameraManager.I.transform.forward + new Vector3(randomPosition.x, randomPosition.y, 0)).normalized;

        return finalDireciton;
    }

    protected void OnHitEffect(RaycastHit hitInfo, float shakeMagnitude)
    {
        CameraManager.I.Shake(0.1f, shakeMagnitude);

        BulletEffect.transform.position = hitInfo.point;
        BulletEffect.transform.forward = hitInfo.normal;
        BulletEffect.Play();

        BulletTrail trail = BulletTrailPool.Get();
        trail.Initialize(FirePosition.position, hitInfo.point);
    }

    public void Knockback(GameObject target, Vector3 dir)
    {
        if(target.GetComponent<CharacterController>() == null)
        {
            target.GetComponent<Rigidbody>().AddForce(dir * KnockbackPower);
        }
        else StartCoroutine(Knockback_Coroutine_with_Controller(target.GetComponent<CharacterController>(), dir));
    }

    public IEnumerator Knockback_Coroutine_with_Controller(CharacterController targetController, Vector3 direction)
    {
        KnockbackElapsedTime = 0f;

        while (KnockbackElapsedTime < KnockbackTimer)
        {
            targetController.Move(direction * KnockbackPower * Time.deltaTime);
            KnockbackElapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

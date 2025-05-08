using UnityEngine;
using System.Collections;
using UnityEngine.Animations.Rigging;

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
    public float TransformLerpSpeed = 20f;

    public Transform FirePosition;
    public ParticleSystem BulletEffect;
    public GameObject BulletTrailPrefab;
    public ParticleSystem MuzzleParticle;
    public GameObjectPool<BulletTrail> BulletTrailPool;
    public LayerMask LayerMask;

    public Vector3 _weaponOffset;

    public RigBuilder RigBuilder;

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
            CameraManager.Instance.IsShooting = false;
        }

        if (Input.GetKeyDown(KeyCode.R)) Reload();
    }

    public virtual void PositionByCamera()
    {
        transform.forward = Camera.main.transform.forward;
        if (CameraManager.Instance.FPSCamera.enabled)
        {
            transform.position = Vector3.Lerp(transform.position, Camera.main.transform.position + Camera.main.transform.TransformDirection(_weaponOffset), Time.deltaTime * TransformLerpSpeed);
            transform.rotation = Camera.main.transform.rotation;
        }
        else if (CameraManager.Instance.TPSCamera.enabled)
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
        PlayerEventManager.Instance.OnReload?.Invoke(true);

        IsReloading = true;
        yield return new WaitForSeconds(2f);
        IsReloading = false;
        CurrentAmmo = MaxAmmo;

        PlayerEventManager.Instance.OnFire?.Invoke();
        PlayerEventManager.Instance.OnReload?.Invoke(false);
    }

    protected Vector3 RandomSpreadDirection()
    {
        Vector2 randomPosition = Random.insideUnitCircle * ((SpreadCoolTime - SpreadElapsedTime) / SpreadCoolTime) * SpreadAmount;
        Vector3 finalDireciton = (CameraManager.Instance.transform.forward + new Vector3(randomPosition.x, randomPosition.y, 0)).normalized;

        return finalDireciton;
    }

    protected void OnHitEffect(RaycastHit hitInfo, float shakeMagnitude)
    {
        CameraManager.Instance.Shake(0.1f, shakeMagnitude);

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
            return;
        }
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null && enemy.CurrentState is DieState) return;
        StartCoroutine(Knockback_Coroutine_with_Controller(target.GetComponent<CharacterController>(), dir));
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

    public void Enter()
    {
        RigBuilder.layers[0].active = true;
        GameManager.Instance.Player.Animator.SetLayerWeight(GameManager.Instance.Player.Animator.GetLayerIndex("ShotLayer"), 1);
        gameObject.SetActive(true);
    }

    public void Exit()
    {
        RigBuilder.layers[0].active = false;
        GameManager.Instance.Player.Animator.SetLayerWeight(GameManager.Instance.Player.Animator.GetLayerIndex("ShotLayer"), 0);
        gameObject.SetActive(false);
    }
}

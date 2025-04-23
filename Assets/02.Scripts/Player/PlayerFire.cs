using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerFire : MonoBehaviour, IPlayerComponent
{
    // 필요 속성
    // - 발사 위치
    public GameObject FirePosition;
    // - 폭탄 프리펩
    public GameObject BombPrefab;
    // - 던지는 힘
    public float ChargeSpeed = 10f;
    private const float _startThrowPower = 10f;
    [SerializeField] private float _throwPower = 0f;
    private const float _maxThrowPower = 40f;
    private bool IsCharging = false;

    private PlayerController _controller;
    private Camera _camera;

    public int BombCount = 3;
    public int MaxBombCount = 3;
    private int _bombPoolIndex = 0;

    private List<GameObject> BombPoolList;

    private const float _fireCooltime = 0.1f;
    private float _elapsedTime = 0.1f;
    public int BulletCount = 50;
    public int MaxBulletCount = 50;
    private float _spreadCoolTime = 2f;
    private float _spreadTime = 0f;
    private float _spreadAmount = 0.07f;

    private bool IsReloading = false;

    public ParticleSystem BulletEffect;
    public GameObject BulletTrailPrefab;
    public List<GameObject> BulletTrailPoolList;
    private int _bulletTrailIndex = 0;


    public Action OnGrenadeChanged;
    public Action OnBulletCountChanged;
    public Action OnReloading;
    public Action StopReloading;

    public TrailRenderer BulletTrail;

    public float KnockbackPower = 1.5f;
    public float KnockbackTimer = 0.5f;
    public float KnockbackElapsedTime = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _camera = Camera.main;
    }

    public void Initialize(PlayerController controller)
    {
        _controller = controller;
        InitializeBombPool();
        InitializeBulletTrailPool();
    }

    public void InitializeBombPool()
    {
        BombPoolList = new List<GameObject>();
        for (int i = 0; i < MaxBombCount * 2; i++)
        {
            GameObject Bomb = Instantiate(BombPrefab);
            Bomb.SetActive(false);
            BombPoolList.Add(Bomb);
        }
    }

    public void InitializeBulletTrailPool()
    {
        BulletTrailPoolList = new List<GameObject>();
        for (int i = 0; i < MaxBulletCount / 2; i++)
        {
            GameObject bulletTrail = Instantiate(BulletTrailPrefab);
            bulletTrail.SetActive(false);
            BulletTrailPoolList.Add(bulletTrail);
        }
    }

    public GameObject GetBombInPool()
    {
        GameObject bomb = BombPoolList[_bombPoolIndex];
        bomb.SetActive(true);

        _bombPoolIndex++;
        BombCount--;
        if (_bombPoolIndex >= BombPoolList.Count) _bombPoolIndex = 0;
        OnGrenadeChanged?.Invoke();

        return bomb;
    }

    public GameObject GetBulletTrailPool(Vector3 startPos, Vector3 targetPos)
    {
        GameObject bullet = BulletTrailPoolList[_bulletTrailIndex];
        bullet.GetComponent<BulletTrail>().Initialize(startPos, targetPos);

        _bulletTrailIndex++;
        if (_bulletTrailIndex >= MaxBulletCount / 2) _bulletTrailIndex = 0;

        bullet.SetActive(true);
        return bullet;
    }

    public bool IsBombLeft()
    {
        if (BombCount > 0) return true;
        return false;
    }

    private void Update()
    {
        FireBomb();
        FireBullet();
        ReLoading();
    }

    private void FireBomb()
    {

        if (Input.GetMouseButtonDown(1))
        {
            if (IsBombLeft())
            {
                IsCharging = true;
                _throwPower = _startThrowPower;
            }
        }

        if (IsCharging)
        {
            _throwPower += Time.deltaTime * ChargeSpeed;
            if (_throwPower >= _maxThrowPower) _throwPower = _maxThrowPower;
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (IsBombLeft())
            {
                GameObject bomb = GetBombInPool();
                bomb.transform.position = FirePosition.transform.position;

                // 4. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기
                Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
                bombRigidbody.AddForce(_camera.transform.forward * _throwPower, ForceMode.Impulse);
                bombRigidbody.AddTorque(Vector3.one);

                CameraManager.I.Shake(0.1f, 0.1f);
                _throwPower = 0f;
                IsCharging = false;
            }
        }

    }

    private void FireBullet()
    {
        if (Input.GetMouseButton(0))
        {
            if (BulletCount > 0)
            {
                CameraManager.I.IsShooting = true;
                if (_elapsedTime >= _fireCooltime)
                {
                    Vector2 randomPosition = UnityEngine.Random.insideUnitCircle * ((_spreadCoolTime - _spreadTime) / _spreadCoolTime) * _spreadAmount;

                    Vector3 finalDireciton = _camera.transform.forward + new Vector3(randomPosition.x, randomPosition.y, 0);
                    Ray ray = new Ray(FirePosition.transform.position, finalDireciton);
                    RaycastHit hitInfo = new RaycastHit();

                    bool isHit = Physics.Raycast(ray, out hitInfo, 50f);
                    if (isHit)
                    {
                        UseBullet();
                        CameraManager.I.Shake(0.1f, 0.1f);

                        BulletEffect.transform.position = hitInfo.point;
                        BulletEffect.transform.forward = hitInfo.normal;
                        BulletEffect.Play();
                        //DrawBulletLine(hitInfo.point);
                        GetBulletTrailPool(FirePosition.transform.position - new Vector3(0f, 0.5f, 0f), hitInfo.point);

                        if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                        {
                            Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                            Damage damage = new Damage();
                            damage.Value = 10;
                            damage.From = this.gameObject;

                            enemy.TakeDamage(damage);
                            Knockback(enemy, finalDireciton);
                        }
                    }
                    else
                    {
                        UseBullet();
                        CameraManager.I.Shake(0.1f, 0.1f);
                        GetBulletTrailPool(FirePosition.transform.position - new Vector3(0f, 0.5f, 0f), FirePosition.transform.position + finalDireciton.normalized * 30f);
                    }
                    _elapsedTime = 0f;
                }

                _spreadTime += Time.deltaTime;
                if (_spreadTime + 0.5f >= _spreadCoolTime)
                {
                    _spreadTime = _spreadCoolTime - 0.5f;
                }
            }
            else
            {
                //ResetBulletLine();
                CameraManager.I.IsShooting = false;
            }
            _elapsedTime += Time.deltaTime;

            // 장전 취소
            if (IsReloading)
            {
                IsReloading = false;
                StopAllCoroutines();
                //ResetBulletLine();
                StopReloading?.Invoke();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _elapsedTime = 0.1f;
            _spreadTime = 0f;
            CameraManager.I.IsShooting = false;
            //ResetBulletLine();
        }
    }

    private void UseBullet()
    {
        BulletCount--;

        OnBulletCountChanged?.Invoke();
    }

    private void ReLoading()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (IsReloading) return;

            if (BulletCount < MaxBulletCount)
            {
                StartCoroutine(ReLoadCoroutine());
            }
        }
    }

    private IEnumerator ReLoadCoroutine()
    {

        OnReloading?.Invoke();
        IsReloading = true;
        yield return new WaitForSeconds(2f);
        IsReloading = false;
        BulletCount = MaxBulletCount;
        OnBulletCountChanged?.Invoke();
        StopReloading.Invoke();
    }

    public void Knockback(Enemy target, Vector3 dir)
    {
        StartCoroutine(Knockback_Coroutine(target.GetComponent<CharacterController>(), dir));
    }

    public IEnumerator Knockback_Coroutine(CharacterController targetController, Vector3 direction)
    {
        KnockbackElapsedTime = 0f;
        
        while(KnockbackElapsedTime < KnockbackTimer)
        {
            targetController.Move(direction * KnockbackPower * Time.deltaTime);
            KnockbackElapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

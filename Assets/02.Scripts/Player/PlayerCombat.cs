using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCombat : APlayerComponent
{
    public Transform MeleeAttackPosition;
    // - 폭탄 프리펩
    public GameObject BombPrefab;
    // - 던지는 힘
    public float ChargeSpeed = 10f;
    private const float _startThrowPower = 10f;
    [SerializeField] private float _throwPower = 0f;
    private const float _maxThrowPower = 40f;
    private bool IsCharging = false;

    private Camera _camera;

    public int BombCount = 3;
    public int MaxBombCount = 3;
    private int _bombPoolIndex = 0;

    private List<GameObject> BombPoolList;

    public List<GameObject> OwnWeaponList;
    public IWeapon CurrentWeapon;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _camera = Camera.main;
        InitializeBombPool();
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

    public GameObject GetBombInPool()
    {
        GameObject bomb = BombPoolList[_bombPoolIndex];
        bomb.SetActive(true);

        _bombPoolIndex++;
        BombCount--;
        if (_bombPoolIndex >= BombPoolList.Count) _bombPoolIndex = 0;
        PlayerEventManager.I.OnThrow?.Invoke();

        return bomb;
    }

    public bool IsBombLeft()
    {
        if (BombCount > 0) return true;
        return false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OwnWeaponList[0].SetActive(true);
            OwnWeaponList[1].SetActive(false);
            CurrentWeapon = OwnWeaponList[0].GetComponent<IWeapon>();
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OwnWeaponList[0].SetActive(false);
            OwnWeaponList[1].SetActive(true);
            CurrentWeapon = OwnWeaponList[1].GetComponent<IWeapon>();
        }

        if (!EventSystem.current.IsPointerOverGameObject() && !CameraManager.I.QVCamera.enabled)
        {
            FireBomb();
            CurrentWeapon.HandleInput();
        }

        if (CameraManager.I.QVCamera.enabled)
        {
            FireBomb();
            CurrentWeapon.HandleInput();
        }
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
                //bomb.transform.position = FirePosition.transform.position;

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
}

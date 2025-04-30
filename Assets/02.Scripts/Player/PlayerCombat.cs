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
    private int _beforeWeaponIndex = -1;
    private int _currentWeaponIndex = 0;
    public IWeapon CurrentWeapon;

    public GameObject UI_SniperZoom;
    public GameObject UI_Crosshair;
    public float ZoomInSize = 15f;
    public float ZoomOutSize = 60f;
    private bool _zoomMode = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerEventManager.I.OnFire += ShotAnimation;
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
        ChangeWeapon();

        if (!EventSystem.current.IsPointerOverGameObject() && !CameraManager.I.QVCamera.enabled)
        {
            CurrentWeapon?.HandleInput();
        }

        if (CameraManager.I.QVCamera.enabled)
        {
            CurrentWeapon?.HandleInput();
        }

        SwitchZoomMode();
    }

    public void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentWeaponIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentWeaponIndex = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentWeaponIndex = 2;
        }
        float wheelInput = Input.GetAxisRaw("Mouse ScrollWheel");
        if (wheelInput > 0.5f)
        {
            _beforeWeaponIndex = _currentWeaponIndex;
            _currentWeaponIndex++;
            if(_currentWeaponIndex >= OwnWeaponList.Count) _currentWeaponIndex = 0;
        }
        else if (wheelInput < -0.5f)
        {
            _beforeWeaponIndex = _currentWeaponIndex;
            _currentWeaponIndex--;
            if (_currentWeaponIndex < 0) _currentWeaponIndex = OwnWeaponList.Count - 1;
        }

        if (_beforeWeaponIndex != _currentWeaponIndex)
        {
            for (int i = 0; i < OwnWeaponList.Count; i++)
            {
                if (i == _currentWeaponIndex)
                {
                    OwnWeaponList[i].SetActive(true);
                    CurrentWeapon = OwnWeaponList[i].GetComponent<IWeapon>();
                }
                else
                {
                    OwnWeaponList[i].SetActive(false);
                }
            }

            PlayerEventManager.I.OnWeaponChanged?.Invoke(_currentWeaponIndex);
            _beforeWeaponIndex = _currentWeaponIndex;
        }
    }

    public void SwitchZoomMode()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _zoomMode = !_zoomMode;
            if (_zoomMode)
            {
                UI_SniperZoom.SetActive(true);
                UI_Crosshair.SetActive(false);
                Camera.main.fieldOfView = ZoomInSize;
            }
            else
            {
                UI_SniperZoom.SetActive(false);
                UI_Crosshair.SetActive(true);
                Camera.main.fieldOfView = ZoomOutSize;
            }
        }
    }
    public void ShotAnimation()
    {
        _controller.Animator.SetTrigger("Shoot");
    }
}

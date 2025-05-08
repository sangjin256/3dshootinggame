using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCombat : APlayerComponent
{
    public Transform MeleeAttackPosition;

    private Camera _camera;

    public List<GameObject> OwnWeaponList;
    private int _beforeWeaponIndex = 0;
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
        PlayerEventManager.Instance.OnFire += ShotAnimation;
        _camera = Camera.main;
    }

    private void Update()
    {
        ChangeWeapon();

        if (!EventSystem.current.IsPointerOverGameObject() && !CameraManager.Instance.QVCamera.enabled)
        {
            CurrentWeapon?.HandleInput();
        }

        if (CameraManager.Instance.QVCamera.enabled)
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
        if (wheelInput > 0)
        {
            _beforeWeaponIndex = _currentWeaponIndex;
            _currentWeaponIndex++;
            if(_currentWeaponIndex >= OwnWeaponList.Count) _currentWeaponIndex = 0;
        }
        else if (wheelInput < 0)
        {
            _beforeWeaponIndex = _currentWeaponIndex;
            _currentWeaponIndex--;
            if (_currentWeaponIndex < 0) _currentWeaponIndex = OwnWeaponList.Count - 1;
        }

        if (_beforeWeaponIndex != _currentWeaponIndex)
        {
            OwnWeaponList[_beforeWeaponIndex].GetComponent<IWeapon>().Exit();
            CurrentWeapon = OwnWeaponList[_currentWeaponIndex].GetComponent<IWeapon>();
            CurrentWeapon.Enter();

            PlayerEventManager.Instance.OnWeaponChanged?.Invoke(_currentWeaponIndex);
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

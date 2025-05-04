using System;
using System.Collections;
using UnityEditor.Build;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    public PlayerDataSO DataSO;

    [SerializeField] private PlayerStatus _status;
    [SerializeField] private PlayerMove _movement;
    [SerializeField] private PlayerRotate _rotation;
    [SerializeField] private PlayerCombat _combat;

    public Animator Animator;
    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        _status.Initialize(this);
        _movement.Initialize(this);
        _rotation.Initialize(this);
        _combat.Initialize(this);

        Animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(Damage damage)
    {
        _status.TakeDamage(damage.Value);
    }

    public bool IsExhausted => _status.IsExhausted();
    public bool IsUsingStamina
    {
        get => _status.IsUsingStamina();
        set => _status.SetUsingStamina(value);
    }

    public void UseStamina(float amount) => _status.UseStamina(amount);
    public float GetHealth() => _status.GetCurrentHealth();
    public float GetStamina() => _status.GetCurrentStamina();

    public float GetMaxHealth() => _status.GetMaxHealth();
    public float GetMaxStamina() => _status.GetMaxStamina();

    public int GetBombCount()
    {
        if(_combat.CurrentWeapon as ThrowSystem != null)
        {
            ThrowSystem throwSystem = _combat.CurrentWeapon as ThrowSystem;
            return throwSystem.ThrowableCount;
        }
        return 0;
    }
    public int GetMaxBombCount()
    {
        if (_combat.CurrentWeapon as ThrowSystem != null)
        {
            ThrowSystem throwSystem = _combat.CurrentWeapon as ThrowSystem;
            return throwSystem.MaxThrowableCount;
        }
        return 0;
    }
    public int GetAmmo()
    {
        if(_combat.CurrentWeapon as IFireable != null)
        {
            BaseFirearm fireable = _combat.CurrentWeapon as BaseFirearm;
            return fireable.CurrentAmmo;
        }
        return 0;
    }
    public int GetMaxAmmo()
    {
        if (_combat.CurrentWeapon as IFireable != null)
        {
            BaseFirearm fireable = _combat.CurrentWeapon as BaseFirearm;
            return fireable.MaxAmmo;
        }
        return 0;
    }
}

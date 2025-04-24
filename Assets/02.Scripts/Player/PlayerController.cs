using System;
using System.Collections;
using UnityEditor.Build;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Data")]
    public PlayerDataSO DataSO;

    [Header("Components")]
    [SerializeField] private PlayerStatus _status;
    [SerializeField] private PlayerMove _movement;
    [SerializeField] private PlayerRotate _rotation;
    [SerializeField] private PlayerFire _combat;

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
    }

    public void TakeDamage(Damage damage)
    {
        _status.TakeDamage(damage.Value);
    }

    // Status 관련 public 속성들
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

    public int GetBombCount() => _combat.BombCount;
    public int GetMaxBombCount() => _combat.MaxBombCount;
    public int GetAmmo() => _combat.CurrentFirearm.CurrentAmmo;
    public int GetMaxAmmo() => _combat.CurrentFirearm.MaxAmmo;
}

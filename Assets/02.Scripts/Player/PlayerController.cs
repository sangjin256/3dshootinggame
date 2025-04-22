using System;
using System.Collections;
using UnityEditor.Build;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerDataSO DataSO;

    public PlayerStat Stat;
    public PlayerMove PlayerMove;
    public PlayerRotate PlayerRotate;
    public PlayerFire PlayerFire;

    public bool IsExhausted = false;
    public bool IsUsingStamina = false;

    public Action OnStaminaChanged;
    public Action OnExhauseted;

    private void Awake()
    {
        Stat = new PlayerStat(DataSO.MaxHealth, DataSO.MaxStamina);

        PlayerMove.Initialize(this);
        PlayerRotate.Initialize(this);
        PlayerFire.Initialize(this);
    }

    private void Update()
    {
        if (Stat.Stamina < Stat.MaxStamina && !IsUsingStamina) ChargeStamina(15f * Time.deltaTime);
    }

    public void UseStamina(float amount)
    {
        Stat.Stamina -= amount;
        if (Stat.Stamina <= 0)
        {
            Stat.Stamina = 0;
            if(!IsExhausted) StartCoroutine(Exhausted());
        }
        OnStaminaChanged?.Invoke();
    }

    public IEnumerator Exhausted()
    {
        IsExhausted = true;
        OnExhauseted?.Invoke();
        yield return new WaitForSeconds(3f);
        IsExhausted = false;
        IsUsingStamina = false;
    }

    public void ChargeStamina(float amount)
    {
        if (!IsExhausted)
        {
            Stat.Stamina += amount;
            if (Stat.Stamina >= Stat.MaxStamina) Stat.Stamina = Stat.MaxStamina;
            OnStaminaChanged?.Invoke();
        }
    } 
}

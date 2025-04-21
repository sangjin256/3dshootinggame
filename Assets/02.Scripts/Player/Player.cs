using System;
using System.Collections;
using UnityEditor.Build;
using UnityEngine;

public class Player : BehaviourSingleton<Player>
{
    public PlayerStat Stat;
    public bool IsExhausted = false;
    public bool IsUsingStamina = false;

    public Action OnStaminaChanged;
    public Action OnExhauseted;

    private void Awake()
    {
        Stat = new PlayerStat(100, 100);
    }

    private void Update()
    {
        if (Stat.Stamina < Stat.MaxStamina && IsUsingStamina == false) ChargeStamina(15f * Time.deltaTime);
    }

    public void UseStamina(float amount)
    {
        Stat.Stamina -= amount;
        if (Stat.Stamina <= 0)
        {
            Stat.Stamina = 0;
            if(IsExhausted == false) StartCoroutine(Exhausted());
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
        if (IsExhausted == false)
        {
            Stat.Stamina += amount;
            if (Stat.Stamina >= Stat.MaxStamina) Stat.Stamina = Stat.MaxStamina;
            OnStaminaChanged?.Invoke();
        }
    } 
}

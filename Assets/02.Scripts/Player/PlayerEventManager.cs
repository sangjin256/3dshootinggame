using System;
using UnityEngine;

public class PlayerEventManager : BehaviourSingleton<PlayerEventManager>
{
    public Action OnThrow;
    public Action OnFire;
    public Action<int> OnWeaponChanged;
    public Action<bool> OnReload;

    public Action<float> OnStaminaChanged;
    public Action<float> OnHealthChanged;
}

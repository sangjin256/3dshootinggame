using UnityEngine;

public class PlayerStat
{
    public float MaxHealth;
    public float MaxStamina;
    public float Health;
    public float Stamina;

    public PlayerStat(float health, float stamina)
    {
        MaxHealth = Health = health;
        MaxStamina = Stamina = stamina;
    }   
}

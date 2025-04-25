using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyStat : MonoBehaviour
{
    public Enemy Enemy;
    public Slider HealthSlider;

    private void Start()
    { 
        Enemy.OnHealthChanged += RefreshHealthUI;
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void Update()
    {
        transform.forward = -CameraManager.I.transform.forward;
    }
    public void RefreshHealthUI()
    {
        float currentHealth = Enemy.Health;
        float maxHealth = Enemy.MaxHealth;
        HealthSlider.value = currentHealth / maxHealth;
    }
}

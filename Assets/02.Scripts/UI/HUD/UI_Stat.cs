using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stat : MonoBehaviour
{
    public Slider StaminaSlider;
    public Slider HealthSlider;
    public Image StaminaBGImage;
    public Image HurtImage;
    Color OriginBGColor;
    Color OriginHurtColor;

    public PlayerController PlayerController;

    private float _hurtElapsedTime = 0;
    public float HurtCooltime;

    private void Start()
    {
        OriginBGColor = StaminaBGImage.color;
        OriginHurtColor = HurtImage.color;
        OriginHurtColor.a = 1.0f;
        PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerEventManager.Instance.OnStaminaChanged += RefreshStaminaUI;
        PlayerEventManager.Instance.OnHealthChanged += RefreshHealthUI;
        PlayerEventManager.Instance.OnHealthChanged += ShowHurtImage;
    }

    public void RefreshStaminaUI(float stamina)
    {
        float currentStamina = PlayerController.GetStamina();
        float maxStamina = PlayerController.GetMaxStamina();
        StaminaSlider.value = currentStamina / maxStamina;
        
        if(stamina <= 0.01f)
        {
            StaminaBGImage.DOFade(0.2f, 0.7f)
                         .SetLoops(4, LoopType.Restart)
                         .OnComplete(() => StaminaBGImage.color = OriginBGColor);
        }
    }

    public void RefreshHealthUI(float health)
    {
        float currentHealth = PlayerController.GetHealth();
        float maxHealth = PlayerController.GetMaxHealth();
        HealthSlider.value = currentHealth / maxHealth;
    }

    public void ShowHurtImage(float health)
    {
        StopAllCoroutines();
        StartCoroutine(Hurt_Coroutine());
    }

    private IEnumerator Hurt_Coroutine()
    {
        HurtImage.color = OriginHurtColor;
        _hurtElapsedTime = 0;

        Color color = OriginBGColor;
        while(_hurtElapsedTime <= HurtCooltime)
        {
            _hurtElapsedTime += Time.deltaTime;
            color.a = (HurtCooltime - _hurtElapsedTime) / HurtCooltime;
            HurtImage.color = color;
            yield return null;
        }
        color.a = 0f;
        HurtImage.color = color;
    }
}

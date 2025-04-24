using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stat : MonoBehaviour
{
    public Slider StaminaSlider;
    public Slider HealthSlider;
    public Image StaminaBGImage;
    Color OriginBGColor;

    public PlayerController PlayerController;

    private void Start()
    {
        OriginBGColor = StaminaBGImage.color;
        PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerEventManager.I.OnStaminaChanged += RefreshStaminaUI;
        PlayerEventManager.I.OnHealthChanged += RefreshHealthUI;
    }

    public void RefreshStaminaUI(float stamina)
    {
        StaminaSlider.value = PlayerController.Stat.Stamina / PlayerController.Stat.MaxStamina;
        if(stamina <= 0.01f) StaminaBGImage.DOFade(0.2f, 0.7f).SetLoops(4, LoopType.Restart).OnComplete(() => StaminaBGImage.color = OriginBGColor);
    }

    public void RefreshHealthUI(float health)
    {
        HealthSlider.value = PlayerController.Stat.Health / PlayerController.Stat.MaxHealth;
    }
}

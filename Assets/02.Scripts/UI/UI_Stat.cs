using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stat : MonoBehaviour
{
    Slider Slider;
    public Image BGImage;
    Color OriginBGColor;

    public PlayerController PlayerController;

    private void Start()
    {
        Slider = GetComponent<Slider>();
        OriginBGColor = BGImage.color;
        PlayerController.OnStaminaChanged += RefreshStaminaUI;
        PlayerController.OnExhauseted += ExhaustedStaminaUI;
    }

    public void RefreshStaminaUI()
    {
        Slider.value = PlayerController.Stat.Stamina / PlayerController.Stat.MaxStamina;
    }

    public void ExhaustedStaminaUI()
    {
        BGImage.DOFade(0.2f, 0.7f).SetLoops(4, LoopType.Restart).OnComplete(() => BGImage.color = OriginBGColor);
    }
}

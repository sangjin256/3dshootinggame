using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stamina : MonoBehaviour
{
    Slider Slider;
    Image BGImage;
    Color OriginBGColor;

    private void Start()
    {
        Slider = GetComponent<Slider>();
        BGImage = transform.GetChild(0).GetComponent<Image>();
        OriginBGColor = BGImage.color;
        Player.I.OnStaminaChanged += RefreshStaminaUI;
        Player.I.OnExhauseted += ExhaustedStaminaUI;
    }

    public void RefreshStaminaUI()
    {
        Slider.value = Player.I.Stat.Stamina / Player.I.Stat.MaxStamina;
    }

    public void ExhaustedStaminaUI()
    {
        BGImage.DOFade(0.2f, 0.7f).SetLoops(4, LoopType.Restart).OnComplete(() => BGImage.color = OriginBGColor);
    }
}

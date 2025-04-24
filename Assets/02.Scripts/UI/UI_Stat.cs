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
        PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerEventManager.I.OnStaminaChanged += RefreshStaminaUI;
    }

    public void RefreshStaminaUI(float stamina)
    {
        Slider.value = PlayerController.Stat.Stamina / PlayerController.Stat.MaxStamina;
        if(stamina <= 0.01f) BGImage.DOFade(0.2f, 0.7f).SetLoops(4, LoopType.Restart).OnComplete(() => BGImage.color = OriginBGColor);
    }
}

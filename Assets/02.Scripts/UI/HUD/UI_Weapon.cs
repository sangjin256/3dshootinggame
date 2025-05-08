using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class UI_Weapon : MonoBehaviour
{
    public PlayerController PlayerController;

    public TextMeshProUGUI CurrentBulletText;
    public TextMeshProUGUI MaxBulletText;
    public TextMeshProUGUI GrenadeText;

    public CanvasGroup BulletCanvasGroup;
    public GameObject ReloadingTextObject;

    private Tween ReloadingTween;

    private void Start()
    {
        PlayerEventManager.Instance.OnThrow += RefreshGrenade;
        PlayerEventManager.Instance.OnFire += RefreshBullet;
        PlayerEventManager.Instance.OnReload += OnReload;
    }

    public void RefreshGrenade()
    {
        int bombCount = PlayerController.GetBombCount();
        int maxBombCount = PlayerController.GetMaxBombCount();

        GrenadeText.text = $"{bombCount}/{maxBombCount}";
    }

    public void RefreshBullet()
    {
        MaxBulletText.text = $"{PlayerController.GetMaxAmmo()}";
        CurrentBulletText.text = $"{PlayerController.GetAmmo()}/";
    }

    public void OnReload(bool isReloading)
    {
        if (isReloading)
        {
            ReloadingTextObject.SetActive(true);
            ReloadingTween = BulletCanvasGroup.DOFade(0f, 0.5f).SetLoops(4, LoopType.Yoyo);
        }
        else
        {
            ReloadingTween?.Kill();
            BulletCanvasGroup.alpha = 1.0f;
            ReloadingTextObject.SetActive(false);
        }
    }
}

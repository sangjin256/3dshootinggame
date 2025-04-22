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
        PlayerController.PlayerFire.OnGrenadeChanged += RefreshGrenade;
        PlayerController.PlayerFire.OnBulletCountChanged += RefreshBullet;
        PlayerController.PlayerFire.OnReloading += OnReLoading;
        PlayerController.PlayerFire.StopReloading += StopReLoading;
    }

    public void RefreshGrenade()
    {
        int bombCount = PlayerController.PlayerFire.BombCount;
        int maxBombCount = PlayerController.PlayerFire.MaxBombCount;

        GrenadeText.text = $"{bombCount}/{maxBombCount}";
    }

    public void RefreshBullet()
    {
        MaxBulletText.text = $"{PlayerController.PlayerFire.MaxBulletCount}";
        CurrentBulletText.text = $"{PlayerController.PlayerFire.BulletCount}/";
    }

    public void OnReLoading()
    {
        ReloadingTextObject.SetActive(true);
        ReloadingTween = BulletCanvasGroup.DOFade(0f, 0.5f).SetLoops(4, LoopType.Yoyo);
    }

    public void StopReLoading()
    {
        ReloadingTween.Kill();
        BulletCanvasGroup.alpha = 1.0f;
        ReloadingTextObject.SetActive(false);
    }
}

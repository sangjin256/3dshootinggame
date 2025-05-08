using UnityEngine;
using System.Collections.Generic;

public class UI_CurrentWeapon : MonoBehaviour
{
    public List<GameObject> WeaponList;

    private void Start()
    {
        PlayerEventManager.Instance.OnWeaponChanged += ChangeWeapon;
    }

    public void ChangeWeapon(int index)
    {
        for(int i = 0; i < WeaponList.Count; i++)
        {
            if (i == index) WeaponList[i].SetActive(true);
            else WeaponList[i].SetActive(false);
        }
    }
}

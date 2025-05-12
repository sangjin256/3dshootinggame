using UnityEngine;
using UnityEngine.UI;

public class LazyHealthBar : MonoBehaviour
{
    public float Speed;
    public Image LazyHealthImage;

    public UI_EnemyStat UI_EnemyStat;

    private void Update()
    {
        if(LazyHealthImage.fillAmount > UI_EnemyStat.HealthSlider.value)
        {
            LazyHealthImage.fillAmount -= Speed * Time.deltaTime;
            if(LazyHealthImage.fillAmount < UI_EnemyStat.HealthSlider.value)
            {
                LazyHealthImage.fillAmount = UI_EnemyStat.HealthSlider.value;
            }
        }
    }

}

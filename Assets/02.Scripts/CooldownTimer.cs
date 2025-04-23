using UnityEngine;
using System.Collections;

public class CooldownTimer : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitTime(3f));
        Debug.Log("안녕하세요.");
    }

    private IEnumerator WaitTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Debug.Log($"{waitTime}이 지났습니다.");
    }
}

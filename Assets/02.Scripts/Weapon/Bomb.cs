using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 목표 : 마우스의 오른쪽 버튼을 누르면 시선이 바라보는 방향으로 수류탄을 던지고 싶다.
    // 1. 수류탄 오브젝트 만들기
    public GameObject ExplosionEffectPrefab;

    // 충돌했을 때
    private void OnCollisionEnter(Collision collision)
    {
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        gameObject.SetActive(false);
    }
}

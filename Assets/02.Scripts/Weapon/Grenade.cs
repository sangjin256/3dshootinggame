using UnityEngine;

public class Grenade : BaseThrowable
{
    public GameObject ExplosionEffectPrefab;

    // 충돌했을 때
    private void OnCollisionEnter(Collision collision)
    {
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        ReturnToPool();
    }
}

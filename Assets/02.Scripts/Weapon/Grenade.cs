using UnityEngine;

public class Grenade : BaseThrowable
{
    public GameObject ExplosionEffectPrefab;

    // 충돌했을 때
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.GetInstanceID() == gameObject.GetInstanceID()) return;
        Debug.Log(collision.collider);
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        ReturnToPool();
    }
}

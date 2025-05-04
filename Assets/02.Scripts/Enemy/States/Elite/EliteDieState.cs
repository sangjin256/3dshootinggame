using UnityEngine;
using UnityEngine.AI;

public class EliteDieState : IState<Enemy>
{
    private float explosionRadius = 5f;
    private float explosionDamage = 30f;

    public void Enter(Enemy enemy)
    {
        PerformDeathExplosion(enemy);
        Object.Destroy(enemy.gameObject, 1f);
    }

    public void Update(Enemy enemy)
    {
    }

    public void Exit(Enemy enemy)
    {
    }

    private void PerformDeathExplosion(Enemy enemy)
    {
        // 폭발 이펙트 생성
        GameObject explosionEffect = Object.Instantiate(enemy.ExplosionPrefab, enemy.transform.position, Quaternion.identity);
        Object.Destroy(explosionEffect, 2f);

        // 폭발 데미지 처리
        Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Damage damage = new Damage();
                damage.Value = (int)explosionDamage;
                damage.From = enemy.gameObject;
                hitCollider.GetComponent<PlayerController>().TakeDamage(damage);
            }
        }
    }
} 
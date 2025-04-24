using System.Collections;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamageable
{
    public int health = 100;
    public int Damage = 30;

    public LayerMask LayerMask;
    public GameObject ExplosionEffect;
    public Rigidbody Rigidbody;
    public float OverlapRadius;
    public float DestroyTime;
    public float ForceAmount;

    private void Start()
    {
        LayerMask = (1 << LayerMask.NameToLayer("Player"));
        LayerMask += (1 << LayerMask.NameToLayer("Enemy"));
    }

    public void TakeDamage(Damage damage)
    {
        if (health <= 0) return;
        health -= damage.Value;
        if (health <= 0) StartCoroutine(OnDeath());
    }

    public IEnumerator OnDeath()
    {
        Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        Vector2 randomPosInCicle = Random.insideUnitCircle;
        Vector3 randomPosition = new Vector3(randomPosInCicle.x, 5f, randomPosInCicle.y);
        Rigidbody.AddForce(randomPosition * ForceAmount, ForceMode.Impulse);
        Rigidbody.AddTorque(Vector3.one);

        Collider[] colliders = Physics.OverlapSphere(transform.position, OverlapRadius, LayerMask);
        if(colliders.Length > 0)
        {
            for(int i = 0; i < colliders.Length; i++)
            {
                Damage damage = new Damage();
                damage.Value = Damage;
                damage.From = this.gameObject;
                colliders[i].GetComponent<IDamageable>().TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(DestroyTime);
        Destroy(this.gameObject);
    }
}

using UnityEngine;

public class EnemyAttacckEvent : MonoBehaviour
{
    public Enemy MyEnemy;

    public void AttackEvent()
    {
        Damage damage = new Damage();
        damage.Value = 10;
        damage.From = MyEnemy.gameObject;
        MyEnemy.TakeDamage(damage);
    }
}

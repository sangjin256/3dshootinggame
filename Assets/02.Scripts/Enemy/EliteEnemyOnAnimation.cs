using UnityEngine;

public class EliteEnemyOnAnimation : MonoBehaviour
{
    public GameObject SlamEffectPrefab;
    public Transform SlamPosition;
    public EliteEnemy EliteEnemy;

    public void PlaySlam()
    {
        GameObject effect = Instantiate(SlamEffectPrefab, SlamPosition.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
        EliteEnemy.OnSpecialAttack?.Invoke();
    }
}

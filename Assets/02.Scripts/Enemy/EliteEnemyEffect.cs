using UnityEngine;

public class EliteEnemyEffect : MonoBehaviour
{
    public GameObject SlamEffectPrefab;

    public void PlaySlamEffect(Vector3 position, Vector3 normal)
    {
        GameObject effect = Instantiate(SlamEffectPrefab, position, Quaternion.identity);
    }
}

using UnityEngine;

public class DomeFollowPlayer : MonoBehaviour
{
    private const float POSITION_Y = -66.28f;

    private void Update()
    {
        Vector3 pos = new Vector3(GameManager.Instance.Player.transform.position.x, POSITION_Y, GameManager.Instance.Player.transform.position.z);
        transform.position = pos;
    }
}

using UnityEngine;

public class PlayerOnAnimation : MonoBehaviour
{
    public void OnAttack()
    {
        PlayerEventManager.Instance.OnSwordAttack?.Invoke();
    }
}

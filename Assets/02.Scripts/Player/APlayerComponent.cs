using UnityEngine;

public abstract class APlayerComponent : MonoBehaviour
{
    protected PlayerController _controller;

    public void Initialize(PlayerController controller)
    {
        _controller = controller;
    }
}

using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TPSCamera : MonoBehaviour
{
    [SerializeField] private float _distance = 5;

    public Vector3 GetPosition()
    {
        transform.eulerAngles = new Vector3(-CameraManager.Instance.RotationY, CameraManager.Instance.RotationX, 0);
        return CameraManager.Instance.ShakePosition + CameraManager.Instance.TPSTarget.position - transform.forward * _distance;
    }

    private void LateUpdate()
    {
        transform.position = GetPosition();
    }
}

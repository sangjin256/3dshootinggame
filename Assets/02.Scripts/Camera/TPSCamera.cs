using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TPSCamera : MonoBehaviour
{
    [SerializeField] private float _distance = 5;

    public Vector3 GetPosition()
    {
        transform.eulerAngles = new Vector3(-CameraManager.I.RotationY, CameraManager.I.RotationX, 0);
        return CameraManager.I.ShakePosition + CameraManager.I.TPSTarget.position - transform.forward * _distance;
    }

    private void LateUpdate()
    {
        transform.position = GetPosition();
    }
}

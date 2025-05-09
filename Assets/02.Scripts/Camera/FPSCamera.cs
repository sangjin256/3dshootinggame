using Unity.VisualScripting;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    public Vector3 GetPosition()
    {
        transform.eulerAngles = new Vector3(-CameraManager.Instance.RotationY, CameraManager.Instance.RotationX, 0);
        return CameraManager.Instance.ShakePosition + CameraManager.Instance.FPSTarget.position;
    }

    private void LateUpdate()
    {
        if (CameraManager.Instance.IsShooting) CameraManager.Instance.RotationY += CameraManager.Instance.BoundY;

        transform.position = GetPosition();
    }
}

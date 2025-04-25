using Unity.VisualScripting;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    public Vector3 GetPosition()
    {
        transform.eulerAngles = new Vector3(-CameraManager.I.RotationY, CameraManager.I.RotationX, 0);
        return CameraManager.I.ShakePosition + CameraManager.I.FPSTarget.position;
    }

    private void LateUpdate()
    {
        if (CameraManager.I.IsShooting) CameraManager.I.RotationY += CameraManager.I.BoundY;

        transform.position = GetPosition();
    }
}

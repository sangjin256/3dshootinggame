using UnityEngine;

public class UI_Crosshair : MonoBehaviour
{
    public float FollowSpeed = 20f;

    private void Update()
    {
        if (CameraManager.Instance.QVCamera.enabled)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        transform.position = Vector3.Lerp(transform.position, Input.mousePosition, Time.deltaTime * FollowSpeed);
    }
}

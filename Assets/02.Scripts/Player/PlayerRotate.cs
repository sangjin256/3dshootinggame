using UnityEngine;

public class PlayerRotate : APlayerComponent
{
    private void Update()
    {
        if (CameraManager.I.QVCamera.enabled)
        {
            Vector3 mouseDirection = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0f);
            mouseDirection = mouseDirection.normalized;
            transform.forward = new Vector3(mouseDirection.x, 0, mouseDirection.y);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, CameraManager.I.RotationX, 0);
        }
            
    }
}

using UnityEngine;

public class UI_Minimap : MonoBehaviour
{
    public Camera MinimapCamera;
    public float MaxSize = 15f;
    public float MinSize = 2f;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OnClickPlus()
    {
        MinimapCamera.orthographicSize--;
        if (MinimapCamera.orthographicSize <= MinSize) MinimapCamera.orthographicSize = MinSize;
    }

    public void OnClickMinus()
    {
        MinimapCamera.orthographicSize++;
        if (MinimapCamera.orthographicSize >= MaxSize) MinimapCamera.orthographicSize = MaxSize;
    }
}

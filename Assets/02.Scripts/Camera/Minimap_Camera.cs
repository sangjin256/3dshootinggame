using UnityEngine;

public class Minimap_Camera : MonoBehaviour
{
    public Transform Target;
    public float YOffset = 15f;

    private void LateUpdate()
    {
        Vector3 newPosition = Target.position;
        newPosition.y += YOffset;
        transform.position = newPosition;

        Vector3 newEulerAngle = Target.eulerAngles;
        newEulerAngle.x = 90;
        newEulerAngle.z = 0;
        transform.eulerAngles = newEulerAngle;
    }
}

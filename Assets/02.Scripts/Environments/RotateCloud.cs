using UnityEngine;

public class RotateCloud : MonoBehaviour
{
    private float rotationY = 0f;
    public float RotateSpeed;
    private void Update()
    {
        rotationY += Time.deltaTime * RotateSpeed;
        transform.rotation = Quaternion.Euler(new Vector3(0, rotationY, 0));

        if (rotationY >= 360f) rotationY = 0f;
    }
}

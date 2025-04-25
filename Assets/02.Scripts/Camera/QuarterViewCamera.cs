using UnityEngine;

public class QuarterViewCamera : MonoBehaviour
{
    [SerializeField] private float _quaterViewAngle;
    [SerializeField] private float _distance;

    public Vector3 GetPosition()
    {
        // 플레이어 중심 위치로부터 이동
        transform.eulerAngles = new Vector3(_quaterViewAngle, 0, 0);
        return CameraManager.I.ShakePosition + CameraManager.I.FPSTarget.transform.position - transform.forward * _distance; 
    }

    private void Update()
    {
        transform.position = GetPosition();
    }
}
using UnityEngine;

public class QuarterViewCamera : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private float followSpeed = 5f; // 따라다니는 속도

    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -10f); // 카메라 위치 오프셋
    [SerializeField] private float rotationSpeed = 5f; // 회전 속도
    [SerializeField] private float minVerticalAngle = 30f; // 최소 수직 각도
    [SerializeField] private float maxVerticalAngle = 60f; // 최대 수직 각도

    [Header("Obstacle Avoidance")]
    [SerializeField] private LayerMask obstacleMask; // 장애물 레이어
    [SerializeField] private float minDistance = 2f; // 최소 거리
    [SerializeField] private float maxDistance = 10f; // 최대 거리

    private Vector3 currentVelocity;
    private float currentRotationX;
    private float currentRotationY;

    private void Start()
    {
        // 초기 카메라 위치 설정
        transform.position = CameraManager.I.QuaterViewTarget.position + offset;
        transform.LookAt(CameraManager.I.QuaterViewTarget.position);

        // 초기 회전값 설정
        currentRotationX = transform.eulerAngles.y;
        currentRotationY = transform.eulerAngles.x;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = CameraManager.I.QuaterViewTarget.position + offset;
        
        // 장애물 체크
        RaycastHit hit;
        Vector3 direction = (targetPosition - CameraManager.I.QuaterViewTarget.position).normalized;
        float distance = Vector3.Distance(targetPosition, CameraManager.I.QuaterViewTarget.position);

        if (Physics.Raycast(CameraManager.I.QuaterViewTarget.position, direction, out hit, distance, obstacleMask))
        {
            targetPosition = hit.point - direction * 0.5f;
        }

        // 부드러운 카메라 이동
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1f / followSpeed);

        // 카메라 회전
        currentRotationX = Mathf.LerpAngle(currentRotationX, CameraManager.I.QuaterViewTarget.eulerAngles.y, rotationSpeed * Time.deltaTime);
        currentRotationY = Mathf.Clamp(currentRotationY, minVerticalAngle, maxVerticalAngle);

        // 카메라 회전 적용
        Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
        transform.rotation = rotation;
    }
}
using NUnit;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform FPSTarget;
    public Transform TPSTarget;

    public FPSCamera FPSCamera;
    public TPSCamera TPSCamera;

    private void Awake()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        FPSTarget = player.GetChild(0).transform;
        TPSTarget = player.GetChild(1).transform;

        FPSCamera?.Initialize(this);
        TPSCamera?.Initialize(this);
    }
}

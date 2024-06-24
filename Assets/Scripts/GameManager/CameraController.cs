using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private Vector3 followOffSet;
    private CinemachineTransposer transposer;
    private void Start()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        followOffSet = transposer.m_FollowOffset;
    }
    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }
    private void HandleMovement()
    {
        Vector3 inputVectorDir = InputManager.Instance.GetCameraMoveVector();
        transform.position += 10f * Time.deltaTime * (transform.forward * inputVectorDir.z + transform.right * inputVectorDir.x);
    }
    private void HandleRotation()
    {
        Vector3 inputVectorRotation = InputManager.Instance.GetCameraRotationVector();
        transform.eulerAngles += 100f * Time.deltaTime * inputVectorRotation;
    }
    private void HandleZoom()
    {
        followOffSet = InputManager.Instance.GetCameraZoomVector(followOffSet);
        followOffSet.y = Mathf.Clamp(followOffSet.y, 1f, 20f);
        transposer.m_FollowOffset = Vector3.Slerp(transposer.m_FollowOffset, followOffSet, Time.deltaTime * 2f);
    }
}

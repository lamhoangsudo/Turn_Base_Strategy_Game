#define USE_NEW_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] private LayerMask mouseLayer;
    private PlayerInputAction playerInputActions;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        playerInputActions = new();
        playerInputActions = new();
        playerInputActions.Camera.Enable();
    }
    public Vector3 GetMouseWorldPosition(out RaycastHit raycastHit)
    {
#if USE_NEW_INPUT_SYSTEM
        Vector3 pos = UtilClass.GetMouseWorldPosititon(Camera.main, mouseLayer, out RaycastHit hitInfo, Mouse.current.position.ReadValue());
        raycastHit = hitInfo;
        return pos;
#else
        Vector3 pos = UtilClass.GetMouseWorldPosititon(Camera.main, mouseLayer, out RaycastHit hitInfo, Input.mousePosition);
        raycastHit = hitInfo;
        return pos;
#endif
    }
    public bool IsMouseButtonDown()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Camera.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }
    public Vector3 GetCameraMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        Vector2 input = playerInputActions.Camera.CameraMovement.ReadValue<Vector2>();
        return new Vector3(input.x, 0, input.y);
#else
        Vector3 inputVectorDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputVectorDir.z++;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            inputVectorDir.z--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVectorDir.x++;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputVectorDir.x--;
        }
        return inputVectorDir;
#endif
    }
    public Vector3 GetCameraRotationVector()
    {
#if USE_NEW_INPUT_SYSTEM
        Vector3 inputVectorRotation = Vector3.zero;
        inputVectorRotation.y += playerInputActions.Camera.CameraRotation.ReadValue<float>();
        return inputVectorRotation;
#else
        Vector3 inputVectorRotation = Vector3.zero;
        if (Input.GetKey(KeyCode.Q))
        {
            inputVectorRotation.y++;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            inputVectorRotation.y--;
        }
        return inputVectorRotation;
#endif
    }
    public Vector3 GetCameraZoomVector(Vector3 followOffSet)
    {
#if USE_NEW_INPUT_SYSTEM
        followOffSet.y += playerInputActions.Camera.CameraZoom.ReadValue<float>();
        return followOffSet;
#else
        if (Input.mouseScrollDelta.y > 0)
        {
            followOffSet.y++;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            followOffSet.y--;
        }
        return followOffSet;
#endif
    }
}

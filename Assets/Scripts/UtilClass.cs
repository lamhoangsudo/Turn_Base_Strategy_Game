using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilClass
{
    public static Vector3 GetMouseWorldPosititon(Camera camera, LayerMask mouseLayer, out RaycastHit raycastHit, Vector3 inputVector)
    {
        raycastHit = new();
        Ray ray = camera.ScreenPointToRay(inputVector);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, mouseLayer.value))
        {
            raycastHit = hitInfo;
            return hitInfo.point;
        }
        return Vector3.zero;
    }
}

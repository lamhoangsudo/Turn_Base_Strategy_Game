using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilClass
{
    public static Vector3 GetMouseWorldPosititon(Camera camera, LayerMask mouseLayer)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, mouseLayer.value))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }
}

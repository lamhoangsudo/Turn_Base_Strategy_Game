using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    public static MouseWorld Instance;
    [SerializeField] private LayerMask mouseLayer;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        this.transform.position = UtilClass.GetMouseWorldPosititon(Camera.main, mouseLayer);
    }
    public Vector3 GetTagetPosititon()
    {
        return UtilClass.GetMouseWorldPosititon(Camera.main, mouseLayer);
    }
}

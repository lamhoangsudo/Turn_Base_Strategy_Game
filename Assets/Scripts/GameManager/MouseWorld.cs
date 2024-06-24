using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    public static MouseWorld Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        this.transform.position = InputManager.Instance.GetMouseWorldPosition(out _);
    }
    /*public Vector3 GetTagetPosititon()
    {
        return InputManager.Instance.GetMouseWorldPosition(mouseLayer);
    }*/
}

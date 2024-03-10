using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private bool invert;
    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    private void LateUpdate()
    {
        if (invert)
        {
            transform.LookAt(transform.position - (cameraTransform.position - transform.position).normalized);
        }
        else
        {
            transform.LookAt(cameraTransform);
        }
    }
}

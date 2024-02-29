using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit selectUnit;
    protected bool isActive;
    protected Action OnActionComplete;
    protected virtual void Awake()
    {
        selectUnit = GetComponent<Unit>();
    }
}

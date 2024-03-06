using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;
    protected Action OnActionComplete;
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
    public abstract string GetNameAction();
    public abstract void GetAction(Action OnActionComplete, Unit unitAction);
    public virtual List<GridPosition> GetListValidGridPosition() 
    {
        return new() { unit.GetGridPosition() };
    }
    public virtual bool IsValidGridPosition(GridPosition gridPosition)
    {
        return true;
    }
    public virtual int GetActionPointCost()
    {
        return 1;
    }
    public bool IsActive()
    {
        return isActive;
    }
    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        OnActionComplete = onActionComplete;
    }
    protected void ActionComplete()
    {
        isActive = false;
        OnActionComplete();
    }
}

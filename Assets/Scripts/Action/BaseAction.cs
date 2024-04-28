using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public Unit unit { get; protected set; }
    protected bool isActive;
    protected Action OnActionComplete;
    public EnemyAIAction aIAction;
    public static event EventHandler OnAnyActionStart;
    public static event EventHandler OnAnyActionComplete;
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
    public abstract string GetNameAction();
    public abstract void GetAction(Action OnActionComplete, Unit unitAction);
    public virtual List<GridPosition> GetListValidGridPosition(out List<GridPosition> range)
    {
        range = new List<GridPosition>();
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
        OnAnyActionStart?.Invoke(this, EventArgs.Empty);
    }
    protected void ActionComplete()
    {
        isActive = false;
        OnActionComplete();
        OnAnyActionComplete?.Invoke(this, EventArgs.Empty);
    }
    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActions = new List<EnemyAIAction>();
        List<GridPosition> validActionGridPositionList = GetListValidGridPosition(out _);
        foreach (GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActions.Add(enemyAIAction);
        }
        if (enemyAIActions.Count > 0)
        {
            enemyAIActions.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActions[0];
        }
        else
        {
            return null;
        }

    }
    public virtual EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return null;
    }
}

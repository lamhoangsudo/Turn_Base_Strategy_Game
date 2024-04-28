using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    private const int MAX_DISTANCE = 1;
    [SerializeField] private LayerMask wall;
    [SerializeField] private float rotationSpeed;
    private State state;
    private float stateTimer;
    private Unit targetUnit;
    public EventHandler OnSwordActionStart;
    public EventHandler OnSwordActionComplete;
    public static EventHandler OnAnySwordActionStart;
    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }
    private void Update()
    {
        if (!isActive) return;
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                Aim();
                break;
            case State.SwingingSwordAfterHit:
                break;
        }
        if (stateTimer < 0)
        {
            StateSwordHandler();
        }
    }
    private void Aim()
    {
        unit.transform.forward = Vector3.Slerp(unit.transform.forward, (targetUnit.transform.position - unit.transform.position).normalized, Time.deltaTime * rotationSpeed);
    }
    private void StateSwordHandler()
    {
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float timeStateSword = 0.5f;
                stateTimer = timeStateSword;
                OnAnySwordActionStart?.Invoke(this, EventArgs.Empty);
                targetUnit.DamageUnit(100);
                break;
            case State.SwingingSwordAfterHit:
                isActive = false;
                OnSwordActionComplete?.Invoke(this, EventArgs.Empty);
                base.ActionComplete();
                break;
        }
    }
    public override bool IsValidGridPosition(GridPosition gridPosition)
    {
        return GetListValidGridPosition(out _).Contains(gridPosition);
    }
    public override List<GridPosition> GetListValidGridPosition(out List<GridPosition> range)
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        List<GridPosition> enemy = GetListValidGridPosition(unitGridPosition, out List<GridPosition> ranges);
        range = ranges;
        return enemy;
    }
    public List<GridPosition> GetListValidGridPosition(GridPosition unitGridPosition, out List<GridPosition> range)
    {
        List<GridPosition> gridPositionsValid = new();
        range = new();
        for (int x = -MAX_DISTANCE; x <= MAX_DISTANCE; x++)
        {
            for (int z = -MAX_DISTANCE; z <= MAX_DISTANCE; z++)
            {
                GridPosition validGridPosition = unitGridPosition + new GridPosition(x, z);
                Debug.Log(validGridPosition);
                if (!LevelGrid.Instance.IsValidGridPosition(validGridPosition)) continue;               
                if (validGridPosition == unitGridPosition) continue;
                if (Physics.Raycast(LevelGrid.Instance.GetGridPosition(unit.GetGridPosition()) + Vector3.up * 1.7f,
                        (LevelGrid.Instance.GetGridPosition(validGridPosition) - LevelGrid.Instance.GetGridPosition(unit.GetGridPosition())).normalized,
                        Vector3.Distance(LevelGrid.Instance.GetGridPosition(validGridPosition) + Vector3.up * 1.7f, LevelGrid.Instance.GetGridPosition(unit.GetGridPosition()) + Vector3.up * 1.7f),
                        wall)) continue;
                range.Add(validGridPosition);
                if (!LevelGrid.Instance.IsUnitOnGridPosition(validGridPosition)) continue;
                if (unit.IsPlayer() == LevelGrid.Instance.GetUnitAtGridPosition(validGridPosition)[0].IsPlayer()) continue;
                {
                    gridPositionsValid.Add(validGridPosition);
                }
            }
        }
        return gridPositionsValid;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction 
        {
            gridPosition = gridPosition,
            actionValue = 200,
        };
    }
    public override void GetAction(Action OnActionComplete, Unit unitAction)
    {
        unit = unitAction;
        if (unit.IsPlayer())
        {
            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetTagetPosititon()))[0];
        }
        else
        {
            Vector3 tagetPosition = LevelGrid.Instance.GetGridPosition(aIAction.gridPosition);
            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(LevelGrid.Instance.GetGridPosition(tagetPosition))[0];
        }
        state = State.SwingingSwordBeforeHit;
        float timeStateSword = 0.7f;
        stateTimer = timeStateSword;
        OnSwordActionStart?.Invoke(this, EventArgs.Empty);
        ActionStart(OnActionComplete);
    }

    public override string GetNameAction()
    {
        return "Sword";
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private int maxMoveDistance;
    private List<GridPosition> listPosition;
    private const float tagetPositionDistance = 0.1f;
    private bool unitIsMoving;
    private const string actionName = "Move";
    public event EventHandler OnStartAction;
    public event EventHandler OnStopAction;
    private int currentIndex;
    private void Start()
    {
    }
    private void Update()
    {
        HandleMovement();
    }
    private void HandleMovement()
    {
        if (!isActive)
        {
            return;
        }
        Debug.Log(currentIndex);
        Vector3 tagetPosition = LevelGrid.Instance.GetGridPosition(listPosition[currentIndex]);
        Vector3 moveDir = (tagetPosition - this.transform.position).normalized;
        if (Vector3.Distance(tagetPosition, this.transform.position) > tagetPositionDistance)
        {
            OnStartAction?.Invoke(this, EventArgs.Empty);
            isActive = true;
            unitIsMoving = true;
            this.transform.position += moveSpeed * Time.deltaTime * moveDir;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotationSpeed);
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        }
        else
        {
            currentIndex++;
            if (currentIndex >= listPosition.Count)
            {
                base.ActionComplete();
                OnStopAction?.Invoke(this, EventArgs.Empty);
                unitIsMoving = false;
            }
        }
    }
    public override bool IsValidGridPosition(GridPosition gridPosition)
    {
        return GetListValidGridPosition().Contains(gridPosition);
    }
    public override List<GridPosition> GetListValidGridPosition()
    {
        List<GridPosition> gridPositionsValid = new();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition validGridPosition = unitGridPosition + new GridPosition(x, z);
                if (LevelGrid.Instance.IsValidGridPosition(validGridPosition)
                    && validGridPosition != unitGridPosition
                    && !LevelGrid.Instance.IsUnitOnGridPosition(validGridPosition)
                    && Pathfinding.instance.IsWalkable(validGridPosition)
                    && Pathfinding.instance.FindPath(unitGridPosition, validGridPosition, out int pathLength) != null
                    && pathLength < maxMoveDistance * 10)
                {
                    gridPositionsValid.Add(validGridPosition);
                };
            }
        }
        return gridPositionsValid;
    }
    public bool UnitIsMoving()
    {
        return unitIsMoving;
    }

    public override string GetNameAction()
    {
        return actionName;
    }

    public override void GetAction(Action onMoveComplete, Unit unitAction)
    {
        unit = unitAction;
        listPosition = new();
        currentIndex = 0;
        if (unit.IsPlayer())
        {
            listPosition = Pathfinding.instance.FindPath(unitAction.GetGridPosition(), LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetTagetPosititon()), out _);
        }
        else
        {
            listPosition = Pathfinding.instance.FindPath(unitAction.GetGridPosition(), aIAction.gridPosition, out _);
        }
        base.ActionStart(onMoveComplete);
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction()
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtPosition * 10
        };
    }
}

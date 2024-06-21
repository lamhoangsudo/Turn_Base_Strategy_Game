using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class InteractAction : BaseAction
{
    private const int MAX_DISTANCE = 1;
    [SerializeField] private LayerMask wall;
    private Door targetDoor;
    private void Update()
    {
        if (!isActive) return;
    }
    public override void GetAction(Action OnActionComplete, Unit unitAction)
    {
        unit = unitAction;
        if (unit.IsPlayer())
        {
            targetDoor = LevelGrid.Instance.GetDoorAtGridPosition(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetTagetPosititon()));
            targetDoor.Interact(ActionComplete);
        }
        else
        {
            Vector3 tagetPosition = LevelGrid.Instance.GetGridPosition(aIAction.gridPosition);
            targetDoor = LevelGrid.Instance.GetDoorAtGridPosition(LevelGrid.Instance.GetGridPosition(tagetPosition));
        }
        ActionStart(OnActionComplete);
    }

    public override string GetNameAction()
    {
        return "Interact";
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction 
        {
            actionValue = 0,
            gridPosition = gridPosition,
        };
    }
    public override bool IsValidGridPosition(GridPosition gridPosition)
    {
        return GetListValidGridPosition(out _).Contains(gridPosition);
    }
    public override List<GridPosition> GetListValidGridPosition(out List<GridPosition> range)
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        List<GridPosition> door = GetListValidGridPosition(unitGridPosition, out List<GridPosition> ranges);
        range = ranges;
        return door;
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
                if (!LevelGrid.Instance.IsValidGridPosition(validGridPosition)) continue;
                if (validGridPosition == unitGridPosition) continue;
                if (Physics.Raycast(LevelGrid.Instance.GetGridPosition(unit.GetGridPosition()) + Vector3.up * 1.7f,
                        (LevelGrid.Instance.GetGridPosition(validGridPosition) - LevelGrid.Instance.GetGridPosition(unit.GetGridPosition())).normalized,
                        Vector3.Distance(LevelGrid.Instance.GetGridPosition(validGridPosition) + Vector3.up * 1.7f, LevelGrid.Instance.GetGridPosition(unit.GetGridPosition()) + Vector3.up * 1.7f),
                        wall)) continue;
                range.Add(validGridPosition);
                if (LevelGrid.Instance.IsUnitOnGridPosition(validGridPosition)) continue;
                if (LevelGrid.Instance.GetDoorAtGridPosition(validGridPosition) == null) continue;
                {
                    gridPositionsValid.Add(validGridPosition);
                }
            }
        }
        return gridPositionsValid;
    }
    private void OnInteractComplete()
    {
        ActionComplete();
    }
}

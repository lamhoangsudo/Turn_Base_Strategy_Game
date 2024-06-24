using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private int MAX_DISTANCE;
    [SerializeField] private int MIN_DISTANCE;
    [SerializeField] private LayerMask wall;
    [SerializeField] private Transform grenadePrefab;
    public Unit targetUnit { get; private set; }
    private bool InsideCircleRange(GridPosition position)
    {
        float min = Mathf.Pow(MIN_DISTANCE + 0.5f, 2);
        float max = Mathf.Pow(MAX_DISTANCE + 0.5f, 2);
        float check = Mathf.Pow((unit.GetGridPosition().x - position.x), 2) + Mathf.Pow((unit.GetGridPosition().z - position.z), 2);
        if (check >= min && check <= max)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override bool IsValidGridPosition(GridPosition gridPosition)
    {
        return GetListValidGridPosition(out _).Contains(gridPosition);
    }
    public override List<GridPosition> GetListValidGridPosition(out List<GridPosition> range)
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        range = new();
        return GetListValidGridPosition(unitGridPosition);
    }
    public List<GridPosition> GetListValidGridPosition(GridPosition unitGridPosition)
    {
        List<GridPosition> gridPositionsValid = new();

        for (int x = -MAX_DISTANCE; x <= MAX_DISTANCE; x++)
        {
            for (int z = -MAX_DISTANCE; z <= MAX_DISTANCE; z++)
            {
                GridPosition validGridPosition = unitGridPosition + new GridPosition(x, z);
                Debug.Log(validGridPosition);
                if (!LevelGrid.Instance.IsValidGridPosition(validGridPosition)) continue;
                //if (!LevelGrid.Instance.IsUnitOnGridPosition(validGridPosition)) continue;
                if (validGridPosition == unitGridPosition) continue;
                //if (unit.IsPlayer() == LevelGrid.Instance.GetUnitAtGridPosition(validGridPosition)[0].IsPlayer()) continue;
                if (!InsideCircleRange(validGridPosition)) continue;
                if (Physics.Raycast(LevelGrid.Instance.GetGridPosition(unit.GetGridPosition()) + Vector3.up * 1.7f,
                        (LevelGrid.Instance.GetGridPosition(validGridPosition) - LevelGrid.Instance.GetGridPosition(unit.GetGridPosition())).normalized,
                        Vector3.Distance(LevelGrid.Instance.GetGridPosition(validGridPosition) + Vector3.up * 1.7f, LevelGrid.Instance.GetGridPosition(unit.GetGridPosition()) + Vector3.up * 1.7f),
                        wall)) continue;
                {
                    gridPositionsValid.Add(validGridPosition);
                }
            }
        }
        return gridPositionsValid;
    }
    public override void GetAction(Action OnThrowActionComplete, Unit unitAction)
    {
        unit = unitAction;
        _ = new Vector3(0, 0, 0);
        Vector3 tagetPosition;
        if (unit.IsPlayer())
        {
            tagetPosition = LevelGrid.Instance.GetGridPosition(LevelGrid.Instance.GetGridPosition(InputManager.Instance.GetMouseWorldPosition(out _)));
        }
        else
        {
            tagetPosition = LevelGrid.Instance.GetGridPosition(aIAction.gridPosition);
            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(LevelGrid.Instance.GetGridPosition(tagetPosition))[0];
            tagetPosition = targetUnit.transform.position;
        }
        Transform grenade = Instantiate(grenadePrefab, unit.transform.position, Quaternion.identity);
        grenade.GetComponent<GrenadeProjectileAction>().SetUp(tagetPosition, OnThrowActionComplete);
        base.ActionStart(OnThrowActionComplete);
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit tagetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition)[0];
        return new EnemyAIAction()
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - tagetUnit.GetHealthAmount()) * 100f)
        };
    }

    public override string GetNameAction()
    {
        return "Grenade";
    }

}

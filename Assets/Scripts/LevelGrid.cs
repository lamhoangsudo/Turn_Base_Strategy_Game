using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public event EventHandler OnUnitMove;
    [SerializeField] private Transform gridDebugPrefab;
    private GridSystem gridSystem;
    public static LevelGrid Instance;
    private void Awake()
    {
        Instance = this;
        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateGridDebugObject(gridDebugPrefab);
    }
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnitAtGridObject(unit);
        OnUnitMove?.Invoke(this, EventArgs.Empty);
    }
    public List<Unit> GetUnitAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).GetUnitAtGridObject();
    }
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnitAtGridObject(unit);
    }
    public GridPosition GetGridPosition(Vector3 position)
    {
        return gridSystem.GetGridPosition(position);
    }
    public void UnitMoveGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
    }
}
